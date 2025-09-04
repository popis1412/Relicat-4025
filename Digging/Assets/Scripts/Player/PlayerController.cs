using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Rendering;
#endif

using UnityEngine;
using UnityEngine.SocialPlatforms;


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
<<<<<<< Updated upstream
=======
    // 모래에 플레이어가 끼었을 경우
    private const float HeadCheckRadius = 0.1f;   // Overlap 반지름
    private float NarrowDigDistance = 0.5f; // 모래에 갇혔을 때 파괴 가능 거리

    float angle;
    float t = 0;
>>>>>>> Stashed changes

    bool isDigging = false;
    bool isGround = false;

    Animator anim;
    [SerializeField] Animator jetpack;

    public AudioSource jetpackAudioSourse;
    private bool isplayingjetpack = false;

    public AudioSource footstepAudioSourse01;
    public AudioSource footstepAudioSourse02;
    private bool switchStepSound = false;

<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
=======
>>>>>>> Stashed changes
    public AudioSource diggingAudioSourse;
    private bool isDigSound = false;

>>>>>>> Stashed changes
    private GameObject jetpackEffect; // 이펙트 오브젝트

    private bool _isStuckInSand = false; // 현재 상태 저장
    public bool ISInSand => _isStuckInSand;

    public BlocksDictionary blocksDictionary;
    private GameObject sandPos;

<<<<<<< Updated upstream
=======
    GameObject block;

    RaycastHit2D hit;

    public bool left { get; private set; }
    public bool right { get; private set; }
    public bool up { get; private set; }
    public bool down { get; private set; }
    public int range;

<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    private void Awake()
    {
        input = new PlayerControl();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        playerScript = GetComponent<Player>();

        blockLayer = LayerMask.GetMask("Block");
        pickAxe = sr.sprite;

        maxHP = playerScript.li_PlayerHearts.Length;
        currentHP = maxHP;

        weapon = FindWeapon();
        weaponSpr = weapon != null ? weapon.GetComponent<SpriteRenderer>().sprite
            : null;

        weapon = FindWeapon();
        weaponSpr = weapon != null ? weapon.GetComponent<SpriteRenderer>().sprite
            : null;

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

<<<<<<< Updated upstream
<<<<<<< Updated upstream
        

        // 단축키 아이템 사용
        if(Input.GetKeyDown(KeyCode.Q))
        {
            SlotManager.Instance.EquipItemByType(ItemType.Bomb);
            Tool.Instance?.UseItem(isGround);
=======
=======
>>>>>>> Stashed changes
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // 좌클릭 - 곡괭이 자동 장착 + 사용
        if(Input.GetMouseButton(0))
        {
            bool equipped = SlotManager.Instance.EquipWeaponByType(WeaponType.Pickaxe);
            if(equipped)
                Tool.Instance.UseWeapon(mousePos, playerScript, WeaponType.Pickaxe);
        }

        // 우클릭 - 드릴 자동 장착 + 사용
        if(Input.GetMouseButton(1))
        {
            bool equipped = SlotManager.Instance.EquipWeaponByType(WeaponType.Drill);
            if(equipped)
                Tool.Instance.UseWeapon(mousePos, playerScript, WeaponType.Drill);
        }

        // 퀵슬롯에 아이템이 있는지 확인 후 그 슬롯의 아이템 장착 및 사용
        if(Input.GetKeyDown(KeyCode.Q))
        {
            SlotManager.Instance.EquipItemByType(ItemType.Bomb, transform);
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            SlotManager.Instance.EquipItemByType(ItemType.Torch);
            Tool.Instance?.UseItem(isGround);
=======
            SlotManager.Instance.EquipItemByType(ItemType.Torch, transform);
>>>>>>> Stashed changes
=======
            SlotManager.Instance.EquipItemByType(ItemType.Torch, transform);
>>>>>>> Stashed changes
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            SlotManager.Instance.EquipItemByType(ItemType.Teleport);
            Tool.Instance?.UseItem(isGround);
=======
            SlotManager.Instance.EquipItemByType(ItemType.Teleport, transform);
>>>>>>> Stashed changes
=======
            SlotManager.Instance.EquipItemByType(ItemType.Teleport, transform);
>>>>>>> Stashed changes
        }

        anim.SetBool("IsGround", isGround);
        anim.SetFloat("MoveSpeed", rb.velocity.magnitude);
        anim.SetFloat("FlySpeed", rb.velocity.magnitude);
        anim.SetBool("IsFlying", isFlying);
        anim.SetBool("IsDigging", isDigging);
    }

    // 모래에 갇혔음을 액션 바인드하기
    private void UpdateSandStatus()
    {
        bool stuckNow = IsStuckInSand();

        if(stuckNow != _isStuckInSand)
        {
            _isStuckInSand = stuckNow;
            HandleSandTrapStateChanged(stuckNow);
        }
    }

    // 플레이어 후처리
    private void HandleSandTrapStateChanged(bool isInSand)
    {
        if(isInSand)
        {
            Debug.Log("플레이어가 모래에 갇혔습니다!");

            if(sandPos != null)
            {
                Vector2 targetPos = sandPos.transform.position;
                transform.position = targetPos;
            }

            // TODO: 여기서 모래 트랩 진입 시 필요한 후처리 실행
            rb.gravityScale = 0;
            col.enabled = false;

            anim.SetBool("SandTrap", true);
        }
        else
        {
            Debug.Log("플레이어가 모래에서 벗어났습니다!");

            // TODO: 모래 탈출 시 필요한 후처리 실행
            rb.gravityScale = 1;
            col.enabled = true;

            anim.SetBool("SandTrap", false);
        }
    }


    private void FixedUpdate()
    {
        // 좌클릭 - 곡괭이 자동 장착 + 사용
        if (Input.GetMouseButton(0))
        {
            SlotManager.Instance.EquipWeaponByType(WeaponType.Pickaxe);
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Tool.Instance.UseWeapon(mouseWorldPos, playerScript);
        }

        // 우클릭 - 드릴 자동 장착 + 사용
        if (Input.GetMouseButton(1))
        {
            SlotManager.Instance.EquipWeaponByType(WeaponType.Drill);
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Tool.Instance?.UseWeapon(mouseWorldPos, playerScript);
        }
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
        float horizontalVelocity = moveInput.x * moveSpeed; // 좌우 속도
        ///<TOOD>
        /// 굴착소년 쿵처럼 일정한 속도로 하고 싶다면 이 코드를 사용
        ///float verticalVelocity = 
        ///    isJumping ? jumpAxis * jumpSpeed  :  // 점프 중이면 가속 누적
        ///    rb.velocity.y;                      // 아니면 기존 y속도 유지(중력)
        ///</TOOD>
        float verticalVelocity = CalculateVerticalVelocity();   // 상하 속도

        // Rigdbody 속도 설정 (y속도는 최대 flySpeed 범위 내로 제한)
        rb.velocity = new Vector2(horizontalVelocity, Mathf.Clamp(verticalVelocity, -flySpeed, flySpeed));

        if(moveInput.x != 0) // 플레이어가 좌우로 움직일때 
        {
            float rotY = moveInput.x > 0 ? 0f : 180f;
            transform.rotation = Quaternion.Euler(0f, rotY, 0f);

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

        print($"isFlying: {isFlying}, verticalVelocity: {Mathf.Clamp(verticalVelocity, -flySpeed, flySpeed)}");
    }

    float gravity = -9.18f;

    // 비행/하강 속도 계산
    private float CalculateVerticalVelocity()
    {
        if(flyInput > 0f)   // W키를 누른 상태
        {
            // 상승 중: 가속
            print("상승 중");
            jetpack.GetComponent<SpriteRenderer>().enabled = true;
            jetpackEffect.SetActive(true);
            jetpack.Play("Anim", -1, 0);
            jetpack.speed = 1;

            // flyAcceeration: 비행 가속도, flyAxis: 비행 입력을 부드럽게 적용하는 것(0 ~ 1 사이 값을 천천히 변화)
            if(verticalSpeed < 0 && isGround)
                verticalSpeed = 0;

            verticalSpeed += flyAcceleration * flyAxis * Time.fixedDeltaTime;
            verticalSpeed = Mathf.Min(verticalSpeed, flySpeed);

        }
        else if(IsStuckInSand() == true)
        {
            verticalSpeed = 0;
        }
        else if(isGround) // W를 땐 상태에서 바닥에 닿았을 때
        {
            verticalSpeed = 0;
        }
        else // W키를 땐 상태
        {
            // 입력 없음: 감속
            print("하강 중");
            jetpack.GetComponent<SpriteRenderer>().enabled = false;
            jetpackEffect.SetActive(false);
            jetpack.speed = 0;

            verticalSpeed += gravity * Time.deltaTime;
        }

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
        return verticalSpeed; // 하강 속도 리턴
    }

    // 클릭한 블록 파괴 처리
    private void HandleDigging()
    {
        List<GameObject> targetBlocks = GetVaildBlocksUnderMouse(hit);

        if(targetBlocks == null || targetBlocks.Count == 0 || !input.Player.Digging.IsPressed())
        {
            isDigging = false;
            t = 0;
            return;
        }

        foreach(var targetBlock in targetBlocks)
        {
            if(targetBlock == null) continue;

            print($"타겟 블록: {targetBlock.transform.position}");

            if(targetBlock.TryGetComponent(out Block block))
            {
                isDigging = true;
                block.BlockDestroy(Time.deltaTime * pickdamage, playerScript);

                //// Dig 사운드
                //// 일반 흙 블록
                if(block.blockType == 0 || block.blockType == 4 || block.blockType == 5)
                {
                    int idx = UnityEngine.Random.Range(5, 9);
                    if(isDigging && isDigSound == false)
                    {
                        diggingAudioSourse.PlayOneShot(SoundManager.Instance.SFXSounds[idx]);
                        isDigSound = true;
                    }
                    if(!isDigging && diggingAudioSourse.isPlaying == true)
                    {
                        diggingAudioSourse.Stop();
                        isDigSound = false;
                    }
                    if(diggingAudioSourse.isPlaying == false)
                    {
                        isDigSound = false;
                    }
                }
                // 광물 블록
                if(block.blockType == 2 || block.blockType == 7 || block.blockType == 8 || block.blockType == 9 || block.blockType == 10 || block.blockType == 11)
                {
                    if(isDigging && isDigSound == false)
                    {
                        diggingAudioSourse.PlayOneShot(SoundManager.Instance.SFXSounds[12]);
                        isDigSound = true;
                    }
                    if(!isDigging && diggingAudioSourse.isPlaying == true)
                    {
                        diggingAudioSourse.Stop();
                        isDigSound = false;
                    }
                    if(diggingAudioSourse.isPlaying == false)
                    {
                        isDigSound = false;
                    }
                }
                // 바위 블록
                if(block.blockType == 3 || block.blockType == -1)
                {
                    int idx = UnityEngine.Random.Range(9, 12);
                    if(isDigging && isDigSound == false)
                    {
                        diggingAudioSourse.PlayOneShot(SoundManager.Instance.SFXSounds[idx]);
                        isDigSound = true;
                    }
                    if(!isDigging && diggingAudioSourse.isPlaying == true)
                    {
                        diggingAudioSourse.Stop();
                        isDigSound = false;
                    }
                    if(diggingAudioSourse.isPlaying == false)
                    {
                        isDigSound = false;
                    }
                }
                // 모래 블록
                if(block.blockType == 6)
                {

                    if(isDigging && isDigSound == false)
                    {
                        diggingAudioSourse.PlayOneShot(SoundManager.Instance.SFXSounds[8]);
                        isDigSound = true;
                    }
                    if(!isDigging && diggingAudioSourse.isPlaying == true)
                    {
                        diggingAudioSourse.Stop();
                        isDigSound = false;
                    }
                    if(diggingAudioSourse.isPlaying == false)
                    {
                        isDigSound = false;
                    }
                }
            }
        }

        pivot = transform.Find("Pivot").position;

        t += Time.fixedDeltaTime * 4f;
        t = Mathf.Clamp01(t);


        angle = Mathf.Lerp(60, -30, t);
        float rad = angle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * 0.1f;

        //print("offset: " + offset + "angle: " + angle + "t: " + t );
        

        if (t >= 1)
        {
            print("t = 0");
            t = 0;
            weapon.transform.position = pivot;
            weapon.transform.rotation = Quaternion.Euler(0f, 0f, -30f);
        }
        weapon.transform.position = pivot + offset;
        weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // 클릭한 블록 파괴 처리
    private void HandleDigging()
    {
        List<GameObject> targetBlocks = GetVaildBlocksUnderMouse(hit);

        if(targetBlocks == null || targetBlocks.Count == 0 || !input.Player.Digging.IsPressed())
        {
            isDigging = false;
            t = 0;
            return;
        }

        foreach(var targetBlock in targetBlocks)
        {
            if(targetBlock == null) continue;

            print($"타겟 블록: {targetBlock.transform.position}");

            if(targetBlock.TryGetComponent(out Block block))
            {
                isDigging = true;
                block.BlockDestroy(Time.deltaTime * pickdamage, playerScript);

                //// Dig 사운드
                //// 일반 흙 블록
                if(block.blockType == 0 || block.blockType == 4 || block.blockType == 5)
                {
                    int idx = UnityEngine.Random.Range(5, 9);
                    if(isDigging && isDigSound == false)
                    {
                        diggingAudioSourse.PlayOneShot(SoundManager.Instance.SFXSounds[idx]);
                        isDigSound = true;
                    }
                    if(!isDigging && diggingAudioSourse.isPlaying == true)
                    {
                        diggingAudioSourse.Stop();
                        isDigSound = false;
                    }
                    if(diggingAudioSourse.isPlaying == false)
                    {
                        isDigSound = false;
                    }
                }
                // 광물 블록
                if(block.blockType == 2 || block.blockType == 7 || block.blockType == 8 || block.blockType == 9 || block.blockType == 10 || block.blockType == 11)
                {
                    if(isDigging && isDigSound == false)
                    {
                        diggingAudioSourse.PlayOneShot(SoundManager.Instance.SFXSounds[12]);
                        isDigSound = true;
                    }
                    if(!isDigging && diggingAudioSourse.isPlaying == true)
                    {
                        diggingAudioSourse.Stop();
                        isDigSound = false;
                    }
                    if(diggingAudioSourse.isPlaying == false)
                    {
                        isDigSound = false;
                    }
                }
                // 바위 블록
                if(block.blockType == 3 || block.blockType == -1)
                {
                    int idx = UnityEngine.Random.Range(9, 12);
                    if(isDigging && isDigSound == false)
                    {
                        diggingAudioSourse.PlayOneShot(SoundManager.Instance.SFXSounds[idx]);
                        isDigSound = true;
                    }
                    if(!isDigging && diggingAudioSourse.isPlaying == true)
                    {
                        diggingAudioSourse.Stop();
                        isDigSound = false;
                    }
                    if(diggingAudioSourse.isPlaying == false)
                    {
                        isDigSound = false;
                    }
                }
                // 모래 블록
                if(block.blockType == 6)
                {

                    if(isDigging && isDigSound == false)
                    {
                        diggingAudioSourse.PlayOneShot(SoundManager.Instance.SFXSounds[8]);
                        isDigSound = true;
                    }
                    if(!isDigging && diggingAudioSourse.isPlaying == true)
                    {
                        diggingAudioSourse.Stop();
                        isDigSound = false;
                    }
                    if(diggingAudioSourse.isPlaying == false)
                    {
                        isDigSound = false;
                    }
                }
            }
        }

        pivot = transform.Find("Pivot").position;

        t += Time.fixedDeltaTime * 4f;
        t = Mathf.Clamp01(t);


        angle = Mathf.Lerp(60, -30, t);
        float rad = angle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * 0.1f;

        //print("offset: " + offset + "angle: " + angle + "t: " + t );
        

        if (t >= 1)
        {
            print("t = 0");
            t = 0;
            weapon.transform.position = pivot;
            weapon.transform.rotation = Quaternion.Euler(0f, 0f, -30f);
        }
        weapon.transform.position = pivot + offset;
        weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // 마우스 위치 기준으로 파괴 가능한 블록 반환
    private List<GameObject> GetVaildBlocksUnderMouse(RaycastHit2D hit)
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

        return CanDigBlocks(playerPos, clickBlockPos, blocksDict);
    }

    private struct DirectionSet
    {
        public Vector2 direction;
        public bool isDiagonal;
        public float snap;
        public float gridStep;

        public DirectionSet(Vector2 dir, bool diagonal, float snapVal, float gridStepVal)
        {
            direction = dir;
            isDiagonal = diagonal;
            snap = snapVal;
            gridStep = gridStepVal;
        }
    }

    private List<GameObject> CanDigBlocks(Vector2 playerPos, Vector2 blockPos, BlocksDictionary blocksDict)
    {
        List<GameObject> diggableBlocks = new();

        Vector2 basePos = new(Mathf.Floor(playerPos.x), Mathf.Floor(playerPos.y));
        Vector2 offset = blockPos - basePos;

        List<DirectionSet> directionSets = new();

        for(int r = 0; r < range; r++)
        {
            float snap = 0.5f + r;
            float gridStep = snap + 1f;

            directionSets.Add(new DirectionSet(new Vector2(-snap, 0.5f), false, snap, gridStep));     // Left
            directionSets.Add(new DirectionSet(new Vector2(gridStep, 0.5f), false, snap, gridStep));  // Right
            directionSets.Add(new DirectionSet(new Vector2(0.5f, gridStep), false, snap, gridStep));  // Up
            directionSets.Add(new DirectionSet(new Vector2(0.5f, -snap), false, snap, gridStep));     // Down

            directionSets.Add(new DirectionSet(new Vector2(-snap, gridStep), true, snap, gridStep));   // Left-Up
            directionSets.Add(new DirectionSet(new Vector2(gridStep, gridStep), true, snap, gridStep));// Right-Up
            directionSets.Add(new DirectionSet(new Vector2(-snap, -snap), true, snap, gridStep));      // Left-Down
            directionSets.Add(new DirectionSet(new Vector2(gridStep, -snap), true, snap, gridStep));   // Right-Down
        }

        // offset이 1칸 거리인 경우만 처리 (snap == 0.5 인 경우만 처리)
        bool foundAdjacent = false;
        DirectionSet adjacentDirSet = default;

        foreach(var dirSet in directionSets)
        {
            if(dirSet.snap == 0.5f && offset == dirSet.direction)
            {
                foundAdjacent = true;
                adjacentDirSet = dirSet;
                break;
            }
        }

        if(!foundAdjacent)
        {
            // 1칸 거리 블록이 아니면 캘 수 없음
            return diggableBlocks;
        }

        // 1칸 거리인 blockPos 기준으로 range만큼 블록 캘 수 있게 처리
        float snapAdj = adjacentDirSet.snap;
        float gridStepAdj = adjacentDirSet.gridStep;

        if(!adjacentDirSet.isDiagonal)
        {
            Vector2 stepDir = adjacentDirSet.direction - new Vector2(0.5f, 0.5f); // 방향 유도

            for(int step = 0; step < range; step++)
            {
                Vector2 digPos = blockPos + stepDir * step;

                if(blocksDict.blockPosition.TryGetValue(digPos, out GameObject block))
                    diggableBlocks.Add(block);
            }
        }
        else
        {
            // 대각선 차단 (1칸 거리 기준)
            bool left = blocksDict.blockPosition.ContainsKey(basePos + new Vector2(-0.5f, 0.5f));
            bool right = blocksDict.blockPosition.ContainsKey(basePos + new Vector2(1.5f, 0.5f));
            bool up = blocksDict.blockPosition.ContainsKey(basePos + new Vector2(0.5f, 1.5f));
            bool down = blocksDict.blockPosition.ContainsKey(basePos + new Vector2(0.5f, -0.5f));

            bool blocked =
                (adjacentDirSet.direction == new Vector2(-snapAdj, gridStepAdj) && (left && up)) ||
                (adjacentDirSet.direction == new Vector2(gridStepAdj, gridStepAdj) && (right && up)) ||
                (adjacentDirSet.direction == new Vector2(-snapAdj, -snapAdj) && (left && down)) ||
                (adjacentDirSet.direction == new Vector2(gridStepAdj, -snapAdj) && (right && down));

            if(!blocked)
            {
                if(blocksDict.blockPosition.TryGetValue(blockPos, out GameObject block))
                    diggableBlocks.Add(block);
            }
        }

        foreach(GameObject block in diggableBlocks)
            Debug.Log("캘 수 있는 블럭의 위치: " + block.transform.position);

        return diggableBlocks;
    }


    // 모래에 갇혔는지 판별
    private bool IsStuckInSand()
    {
        // 현재 플레이어 위치
        Vector2 playerCenter = new Vector2(
            Mathf.Floor(transform.position.x) + 0.5f,
            Mathf.Floor(transform.position.y) + 0.5f
        );

        // 체크할 위치들: 자기 위치 + 위쪽 1칸
        Vector2[] checkPositions = new Vector2[]
        {
            playerCenter,               // 자기 위치
        };

        foreach(var pos in checkPositions)
        {
            if(blocksDictionary.blockPosition.TryGetValue(pos, out GameObject blockObj))
            {
                Block block = blockObj.GetComponent<Block>();
                if(block != null && block.blockType == 6) // 6번이 모래라면
                {
                    sandPos = blockObj;
                    return true;
                }
            }
        }

        sandPos = null;
        return false;
    }

    // 피해 처리
    public void TakeDamage(int damage, Transform attacker)
    {
        currentHP -= damage;
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[27]);
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
        
>>>>>>> Stashed changes
=======
        
>>>>>>> Stashed changes
        StartCoroutine(DamageEffect(attacker.position));

        if(damage > 0)
        {
            playerScript.LostPlayerLife(currentHP);
        }
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
        
>>>>>>> Stashed changes
=======
        
>>>>>>> Stashed changes
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
        transform.position = new Vector3(15.5f, 0.5f, 0f);
        sr.color = Color.white;
        currentHP = maxHP;
        playerScript.AddPlayerLife(currentHP);
    }

<<<<<<< Updated upstream
=======
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
                playerScript.Inventory.SellItem(item);
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

>>>>>>> Stashed changes
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
                if(collision.gameObject.CompareTag("Floor"))
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
                if(collision.gameObject.CompareTag("Floor"))
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
        if(collision.gameObject.CompareTag("Floor"))
        {
            isGround = false;
        }
    }
}