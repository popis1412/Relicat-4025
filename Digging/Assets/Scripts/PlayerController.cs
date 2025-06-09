using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // Component References
    PlayerControl input;
    Rigidbody2D rb;
    Collider2D col;
    Player playerScript;
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
    Sprite pickAxe;
    public float pickdamage = 1f;
    Sprite weaponSpr;
    [SerializeField] GameObject weapon;
    Vector3 pivot;

    // Damage System
    int maxHP;
    int currentHP = 0;

    // Block Detection
    private int blockLayer;
    // 모래에 플레이어가 끼었을 경우
    private const float HeadCheckRadius = 0.1f;   // Overlap 반지름
    private const float NarrowDigDistance = 0.5f; // 모래에 갇혔을 때 파괴 가능 거리

    float angle;
    float t = 0;

    bool isDigging;

    Animator anim;

    private void Awake()
    {
        input = new PlayerControl();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        playerScript = GetComponent<Player>();

        blockLayer = LayerMask.GetMask("Block");
        pickAxe = sr.sprite;

        maxHP = playerScript.li_PlayerHearts.Count;
        currentHP = maxHP;
        playerSize = sr.size.y;
        bombSize = bomb.GetComponent<SpriteRenderer>().size.y;
        torchSize = torch.GetComponent<SpriteRenderer>().size.y;

        weapon = FindWeapon();
        weaponSpr = weapon != null ? weapon.GetComponent<SpriteRenderer>().sprite
            : null;

        anim = GetComponent<Animator>();
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

    private void Update()
    {
        // 단축키 아이템 사용
        if(Input.GetKeyDown(KeyCode.Alpha1))
            UseItem(0, bomb, GetBombPosition());
        if(Input.GetKeyDown(KeyCode.Alpha2))
            UseItem(1, torch, GetTorchPosition());

        anim.SetFloat("MoveSpeed", rb.velocity.magnitude);
        anim.SetFloat("FlySpeed", rb.velocity.y);
        anim.SetBool("IsFlying", isFlying);
        anim.SetBool("IsDigging", isDigging);
    }

    private void FixedUpdate()
    {
        UpdateJumpAxisSmoothly();
        HandleMovement();
        HandleDigging();
        pivot = transform.GetChild(3).GetComponent<Transform>().position;
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
            float rotY = moveInput.x > 0 ? 0f : 180f;
            transform.rotation = Quaternion.Euler(0f, rotY, 0f);
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
            verticalSpeed += flyAcceleration * flyAxis * Time.fixedDeltaTime;
            verticalSpeed = Mathf.Min(verticalSpeed, flySpeed);
        }
        else
        {
            // 입력 없음: 감속
            //print("하강 중");
            verticalSpeed -= deceleration * Time.fixedDeltaTime;
            verticalSpeed = Mathf.Max(0f, verticalSpeed);
        }

        //print(verticalSpeed);

        // 상승 중이면 상속도 우선, 아니면 중력 등 자연스러운 하강 유지
        return isFlying || rb.velocity.y > 0f ? verticalSpeed : rb.velocity.y;
    }

    // 클릭한 블록 파괴 처리
    private void HandleDigging()
    {
        GameObject targetBlock = GetVaildBlockUnderMouse();

        if(!input.Player.Digging.IsPressed() || targetBlock == null)
        {
            isDigging = false;
            t = 0;
            return;
        }

        t += Time.fixedDeltaTime * 2;
        t = Mathf.Clamp01(t);

        angle = Mathf.Lerp(30, -60, t);
        float rad = angle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * 0.1f;

        print("offset: " + offset + "angle: " + angle + "t: " + t);

        if(t >= 1)
        {
            print("t = 0");
            t = 0;
            weapon.transform.position = pivot;
            weapon.transform.rotation = Quaternion.Euler(0f, 0f, -30f);
        }
        weapon.transform.position = pivot + offset;
        weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (targetBlock.TryGetComponent(out Block block))
        {
            isDigging = true;
            block.BlockDestroy(Time.deltaTime * pickdamage, playerScript);
        }
    }

    // 마우스 위치 기준으로 파괴 가능한 블록 반환
    private GameObject GetVaildBlockUnderMouse()
    {
        // 마우스 레이저
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        // 콜라이더가 없(배경, 빈 화면)거나 블록인 경우만 캐기
        if(hit.collider == null || !hit.collider.CompareTag("Block"))
            return null;

        Vector2 playerPos = new (transform.position.x, transform.position.y);
        Vector2 hitPos = new (hit.transform.position.x, hit.transform.position.y);

        // 캐기 거리
        float distance = Mathf.Sqrt(Mathf.Pow(hitPos.x - playerPos.x, 2) + Mathf.Pow(hitPos.y - playerPos.y, 2));

        // 캐릭터 기준 블록을 캘 수 있는 최대 거리
        float digDistance = IsStuckInSand() ?
            NarrowDigDistance :     // 모래만 캘 수 있는 범위(0.5)
            1.5f / transform.localScale.x;

        return distance < digDistance ? hit.collider.gameObject : null;

        //print($"block Distance: {DigDistance}, Character Distance: {distance}");
    }

    // 모래에 갇혔는지 판별
    private bool IsStuckInSand()
    {
        // 머리 위 오버랩 감지
        Vector2 headCheckPosition = (Vector2)transform.position + new Vector2(0, HeadCheckRadius);

        // 캐릭터도 감지되기 때문에 Layer를 Block으로 해 놓아야 함.
        Collider2D hit = Physics2D.OverlapCircle(headCheckPosition, HeadCheckRadius, blockLayer);

        if(hit == null)
            return false;

        if(hit.TryGetComponent(out Block block) && block.blockType == 6)
        {
            return block.blocksDictionary.blockPosition.ContainsKey(block.transform.position);
        }

        return false;
    }

    // 피해 처리
    public void TakeDamage(int damage, Transform attacker)
    {
        currentHP -= damage;

        playerScript.LostPlayerLife(currentHP);

        if(currentHP <= 0)
        {
            sr.color = Color.red;
            Die();
        }
        else
        {
            StartCoroutine(DamageEffect(attacker.position));
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
            transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        yield return new WaitForSeconds(0.5f);
        sr.color = Color.white;
    }

    // 사망 처리
    void Die()
    {
        transform.position = new Vector3(15.5f, 0.5f, 0f);
        sr.color = Color.white;
        currentHP = maxHP;
    }

    // 아이템 사용 로직
    private void UseItem(int index, GameObject itemPrefab, Vector3 position)
    {
        var item = playerScript.UseItems[index];

        if(item != null && item.count > 0)
        {
            Instantiate(itemPrefab, position, Quaternion.identity);
            item.count--;
            Debug.Log($"아이템 사용: {item.itemName}, 남은 개수: {item.count}");
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
        weaponSpr = torch.GetComponent<SpriteRenderer>().sprite;
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
}