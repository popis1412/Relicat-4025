using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static Tool;


public class PlayerController : MonoBehaviour
{
    // Component References
    public PlayerControl input;
    Rigidbody2D rb;
    Collider2D col;
    [SerializeField] Player playerScript;
    SpriteRenderer sr;

    // Movement
    private Vector2 moveInput;
    private float flyInput;
    private float flyAxis;          // 비행 시 부드럽게 처리하기 위한 보간 값
    private float verticalSpeed;    // Y축 속도
    private bool isFlying;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float flyAcceleration = 10f;   // 비행 가속도
    [SerializeField] private float flySpeed = 8f;           // 최대 비행 속도
    [SerializeField] private float deceleration = 15f;      // 비행 중 하강 시 감속도
    //[SerializeField] private float maxHangDistance = 0.2f;  // 블록에 걸쳐 있을 때 파괴 가능 거리

    // Use Items
    [Header("사용 아이템들")]
    [SerializeField] GameObject bomb;
    [SerializeField] GameObject torch;

    // Size
    float playerSize;
    float bombSize;
    float torchSize;

    // Weapons
    public float pickdamage = 1f;

    // Damage System
    int maxHP;
    int currentHP = 0;

    // Block Detection
    private int blockLayer;
    // 모래에 플레이어가 끼었을 경우
    private const float HeadCheckRadius = 0.1f;   // Overlap 반지름
    private float NarrowDigDistance = 0.5f; // 모래에 갇혔을 때 파괴 가능 거리

    float angle;
    float t = 0;

    public bool isDie = false;

    bool isDigging = false;
    bool isGround = false;

    Animator anim;
    [SerializeField] Animator jetpack;

    public AudioSource jetpackAudioSourse;
    private bool isplayingjetpack = false;

    public AudioSource footstepAudioSourse01;
    public AudioSource footstepAudioSourse02;
    private bool switchStepSound = false;

    
    private bool isDigSound = false;

    private GameObject jetpackEffect; // 이펙트 오브젝트


    // 콜라이더 제어용 이벤트 선언
    public static event Action OnSandEnter;
    public static event Action OnSandExit;
    private bool isInSand = false; // 이전 모래 상태 저장

    public BlocksDictionary blocksDictionary;
    private GameObject sandPos;

    GameObject block;
    Tool tool;

    RaycastHit2D hit;

    public bool left { get; private set; }
    public bool right { get; private set; }
    public bool up { get; private set; }
    public bool down { get; private set; }

    public Vector2 range = Vector2.one;

    private void Awake()
    {
        isDie = false;
        input = new PlayerControl();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        playerScript = GetComponent<Player>();
        tool = FindObjectOfType<Tool>();

        blockLayer = LayerMask.GetMask("Block");

        maxHP = playerScript.li_PlayerHearts.Length;
        currentHP = maxHP;
        playerSize = sr.size.y;
        bombSize = bomb.GetComponent<SpriteRenderer>().size.y;
        torchSize = torch.GetComponent<SpriteRenderer>().size.y;

        anim = GetComponent<Animator>();

        jetpack = transform.Find("jetpack/Anim").GetComponent<Animator>();

        blocksDictionary = GameObject.Find("BlocksDictionary").GetComponent<BlocksDictionary>();
    }

    // 입력 시스템 활성화 및 입력 이벤트 등록
    private void OnEnable()
    {
        input.Enable();

        input.Player.Move.performed += ctx => moveInput = IsStuckInSand() ? Vector2.zero : ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        input.Player.Jump.started += ctx => isFlying = true;
        input.Player.Jump.performed += ctx => flyInput = IsStuckInSand() ? 0f : ctx.ReadValue<float>();
        input.Player.Jump.canceled += ctx => { flyInput = 0; isFlying = false; };
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        jetpackEffect = GameObject.Find("Ef_Jetpack");
    }

    private void Update()
    {
        UpdateSandStatus();

        // 좌클릭 - 곡괭이 자동 장착 + 사용
        if(Input.GetMouseButton(0))
        {
            SlotManager.Instance.EquipWeaponByType(WeaponType.Pickaxe);
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tool.UseWeapon(mouseWorldPos, playerScript);
        }

        // 우클릭 - 드릴 자동 장착 + 사용
        if(Input.GetMouseButton(1))
        {
            SlotManager.Instance.EquipWeaponByType(WeaponType.Drill);
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tool?.UseWeapon(mouseWorldPos, playerScript);
        }

        // 단축키 아이템 사용
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SlotManager.Instance.EquipItemByType(ItemType.Bomb);
            tool?.UseItem();
        }
            
        if(Input.GetKeyDown(KeyCode.E))
        {
            SlotManager.Instance.EquipItemByType(ItemType.Torch);
            tool?.UseItem();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SlotManager.Instance.EquipItemByType(ItemType.Teleport);
            tool?.UseItem();
        }

        anim.SetBool("IsGround", isGround);
        anim.SetFloat("MoveSpeed", rb.velocity.magnitude);
        anim.SetFloat("FlySpeed", rb.velocity.magnitude);
        anim.SetBool("IsFlying", isFlying);
        anim.SetBool("IsDigging", isDigging);
    }

    private void UpdateSandStatus()
    {
        isInSand = IsStuckInSand();
        if(isInSand)
        {
            Debug.Log("플레이어가 모래에 갇혔습니다!");
            col.enabled = false;
            Vector2 targetPos = sandPos.transform.position;
            rb.gravityScale = 0;
            transform.position = targetPos;
            // 필요한 후처리: 예) 애니메이션 재생, 이동 종료 이벤트 등
            
            anim.SetBool("SandTrap", true);

        }
        else
        {
            Debug.Log("플레이어가 모래에서 벗어났습니다!");
            rb.gravityScale = 1;
            col.enabled = true;
            anim.SetBool("SandTrap", false);
        }
    }

    private void FixedUpdate()
    {
        UpdateJumpAxisSmoothly();
        HandleMovement();
    }

    // 비행 입력을 부드럽게 적용(0 ~ 1 사이 값을 천천히 변화)
    private void UpdateJumpAxisSmoothly()
    {
        flyAxis = Mathf.Lerp(flyAxis, flyInput, Time.deltaTime * flySpeed);
    }

    // 이동 처리
    private void HandleMovement()
    {
        float horizontalVelocity = moveInput.x * moveSpeed;
        ///<TOOD>
        /// 굴착소년 쿵처럼 일정한 속도로 하고 싶다면 이 코드를 사용
        ///float verticalVelocity = 
        ///    isJumping ? jumpAxis * jumpSpeed  :  // 점프 중이면 가속 누적
        ///    rb.velocity.y;                      // 아니면 기존 y속도 유지(중력)
        ///</TOOD>
        float verticalVelocity = CalculateVerticalVelocity();

        // Rigdbody 속도 설정 (y속도는 최대 flySpeed 범위 내로 제한)
        rb.velocity = new Vector2(horizontalVelocity, Mathf.Clamp(verticalVelocity, -flySpeed, flySpeed));

        if(moveInput.x != 0)
        {
            Vector3 newScale = transform.localScale;
            newScale.x = moveInput.x > 0 ? 0.9f : -0.9f;
            transform.localScale = newScale;

            if (switchStepSound && !footstepAudioSourse02.isPlaying && isGround)
            {
                switchStepSound = !switchStepSound;
                footstepAudioSourse01.Play();
                
                
            }
            else if (!switchStepSound && !footstepAudioSourse01.isPlaying && isGround)
            {
                switchStepSound = !switchStepSound;
                footstepAudioSourse02.Play();
            }
            
        }
        //print($"isFlying: {isFlying}, verticalVelocity: {verticalVelocity}");
    }

    // 비행/하강 속도 계산
    private float CalculateVerticalVelocity()
    {
        if(flyInput > 0f)
        {
            // 상승 중: 가속
            //print("상승 중");
            jetpack.GetComponent<SpriteRenderer>().enabled = true;
            jetpackEffect.SetActive(true);
            jetpack.Play("Anim", -1, 0);
            jetpack.speed = 1;
            verticalSpeed += flyAcceleration * flyAxis * Time.fixedDeltaTime;
            verticalSpeed = Mathf.Min(verticalSpeed, flySpeed);

        }
        else
        {
            // 입력 없음: 감속
            //print("하강 중");
            jetpack.GetComponent<SpriteRenderer>().enabled = false;
            jetpackEffect.SetActive(false);
            jetpack.speed = 0; 
            verticalSpeed -= deceleration * Time.fixedDeltaTime;
            verticalSpeed = Mathf.Max(0f, verticalSpeed);
        }

        //print(verticalSpeed);

        if (isFlying && isplayingjetpack == false)
        {
            isplayingjetpack = true;
            jetpackAudioSourse.Play();
        }
        else if(!isFlying)
        {
            isplayingjetpack = false;
            jetpackAudioSourse.Stop();
        }

        // 상승 중이면 상속도 우선, 아니면 중력 등 자연스러운 하강 유지
        return isFlying || rb.velocity.y > 0f ? verticalSpeed : rb.velocity.y;
    }

    // 마우스 위치 기준으로 파괴 가능한 블록 반환
    public List<GameObject> GetVaildBlocksUnderMouse(RaycastHit2D hit)
    {
        if(hit.collider == null || !hit.collider.CompareTag("Block"))
            return null;

        Vector2 playerPos = transform.position;
        Vector2 clickBlockPos = hit.transform.position;

        Vector2Int playerGrid = new(
            Mathf.FloorToInt(playerPos.x),
            Mathf.FloorToInt(playerPos.y)
        );
        Vector2Int blockGrid = new(
            Mathf.FloorToInt(clickBlockPos.x),
            Mathf.FloorToInt(clickBlockPos.y)
        );

        int dx = blockGrid.x - playerGrid.x;
        int dy = blockGrid.y - playerGrid.y;

        if(IsStuckInSand())
        {
            if(dx == 0 && dy == 0)
            {
                return new List<GameObject> { sandPos };
            }
            else
            {
                return null;
            }
        }

        var blocksDict = hit.collider.GetComponent<Block>().blocksDictionary;

        //return CanDigBlocks(playerPos, clickBlockPos, blocksDict, range);
        return null;
    }

    // 모래에 갇혔는지 판별
    private bool IsStuckInSand()
    {
        float snap = 0.5f;
        float gridStep = snap + 1f;
        Vector2 playerPos = transform.position;

        Vector2 basePos = new(Mathf.Floor(playerPos.x), Mathf.Floor(playerPos.y));
        Vector2 pos = basePos + new Vector2(snap, snap);    // 15 + 0.5 = 15.5, 0 + 0.5 = 0.5, -1 + 0.5 = -0.5

        print($"Sand | Pos: {pos}, playerPos: {playerPos}, snap: {snap}");

        GameObject posBlockObj = null;
        Block posBlock = null;

        // 현재 위치 블럭 체크
        if(blocksDictionary.blockPosition.TryGetValue(pos, out GameObject foundPosBlock))
        {
            posBlockObj = foundPosBlock;
            posBlock = posBlockObj.GetComponent<Block>();
            print("모래 블럭 찾음 " + posBlock.transform.position);

            if(posBlock != null && posBlock.blockType == 6)
            {
                sandPos = posBlockObj;
                return true;
            }
        }

        // 아무 조건도 만족하지 않을 경우
        sandPos = null;
        return false;
    }

    // 피해 처리
    public void TakeDamage(int damage, Transform attacker)
    {
        currentHP -= damage;
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[27]);
        StartCoroutine(DamageEffect(attacker.position));

        if(damage > 0)
        {
            playerScript.LostPlayerLife(currentHP);
        }
    }

    // 피격 시 색깔 변화와 넉백
    private IEnumerator DamageEffect(Vector2 targetPos)
    {
        sr.color = Color.red;

        // 방향 결정 (좌/우)
        int dir = (transform.position.x - targetPos.x) > 0 ? 1 : -1;

        Vector3 knockback = new Vector3(dir, 0.5f, 0f);

        float duration = 0.2f;
        Vector3 start = transform.position;
        Vector3 end = start + knockback;
        float elapsed = 0f;

        while(elapsed < duration)
        {
            Vector2 nextPos = Vector2.Lerp(start, end, elapsed / duration);
            rb.MovePosition(nextPos);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(end);

        yield return new WaitForSeconds(0.5f);
        sr.color = Color.white;
    }

    // 사망 처리
    public void Die()
    {
        isDie = true;
        transform.position = new Vector3(15.5f, 0.5f, 0f);
        sr.color = Color.white;
        currentHP = maxHP;
        playerScript.AddPlayerLife(currentHP);
    }

    // 아이템 사용 로직
    private void UseItem(int index, GameObject itemPrefab, Vector3 position)
    {
        var item = playerScript.UseItems[index];

        if(item != null && item.count > 0 && isGround)
        {
            Instantiate(itemPrefab, position, Quaternion.identity);
            item.count--;
            playerScript.Inventory.FreshSlot();
            Debug.Log($"아이템 사용: {item.itemName}, 남은 개수: {item.count}");
            if(index == 0)
            {
                SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[18]);
            }
            else if(index == 1)
            {
                SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[14]);
            }
            if(item.count <= 0)
            {
                item.count = 0;
                playerScript.Inventory.RemoveItem(item);
            }

        }
        else
        {
            Debug.LogWarning($"{item?.itemName ?? "아이템"}이 없습니다.");
        }
    }

    // 폭탄 위치 계산
    private Vector3 GetBombPosition() => transform.position + Vector3.up * (playerSize - bombSize);

    // 횃불 생성 위치 계산
    private Vector3 GetTorchPosition()
    {
        return new Vector3(Mathf.Round(transform.position.x - 0.5f) + 0.5f, transform.position.y - (playerSize - torchSize));
    }

    // 무기 찾기
    GameObject FindWeapon()
    {
        foreach(Transform child in transform)
        {
            if(child.CompareTag("Weapon"))
                return child.gameObject;
        }

        return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(ContactPoint2D contact in collision.contacts)
        {
            if(contact.normal.y > 0.5f)
            {
                if(collision.gameObject.CompareTag("Block"))
                {
                    isGround = true;
                    break;
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                if (collision.gameObject.CompareTag("Block"))
                {
                    isGround = true;
                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Block"))
        {
            isGround = false;
        }
    }
}