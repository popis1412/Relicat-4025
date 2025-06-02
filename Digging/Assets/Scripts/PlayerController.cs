using Unity.VisualScripting;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private PlayerControl input;
    private Rigidbody2D rb;
    private Collider2D col;
    private Player playerScript;

    private Vector2 moveInput;
    private float flyInput;
    private float flyAxis; // 상승 입력 부드럽게 보간할 값
    private float verticalSpeed;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float flyAcceleration = 10f;
    [SerializeField] private float flySpeed = 8f;          // 상승 속도(최대 상승 속도)
    [SerializeField] private float deceleration = 15f;

    [SerializeField] private float maxHangDistance = 0.5f; // 블록에 걸쳐 떠 있을 수 있는 최대 거리

    private bool isFlying;
    private int blockLayer;

    // 모래에 플레이어가 끼었을 경우
    private const float HeadCheckYOffset = 0.2f;  // 레이저 Y Offset
    private const float HeadCheckRadius = 0.1f;   // Overlap 반지름
    private const float NarrowDigDistance = 0.5f; // 캘 수 있는 범위

    public float pickdamage = 10f;


    private void Awake()
    {
        input = new PlayerControl();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        playerScript = GetComponent<Player>();

        blockLayer = LayerMask.GetMask("Block");
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Move.performed += ctx => moveInput = IsStuckInSand() ? Vector2.zero : ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        input.Player.Jump.started += ctx => isFlying = true;
        input.Player.Jump.performed += ctx => flyInput = IsStuckInSand() ? 0f : ctx.ReadValue<float>();
        input.Player.Jump.canceled += ctx =>
        {
            flyInput = 0;
            isFlying = false;
        };
    }

    private void FixedUpdate()
    {
        UpdateJumpAxisSmoothly();
        HandleMovement();
        HandleDigging();
    }

    // 상승 입력을 부드럽게 보간(0 ~ 1 사이 값을 천천히 변화)
    private void UpdateJumpAxisSmoothly()
    {
        flyAxis = Mathf.Lerp(flyAxis, flyInput, Time.deltaTime * flySpeed);
    }

    // 플레이어 이동 및 상승(수직 속도) 처리
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

        float playerRot;

        if (moveInput.x > 0)
        {
            playerRot = 0f;
            transform.rotation = Quaternion.Euler(0f, playerRot, 0f);
        }
        else if(moveInput.x < 0)
        {
            playerRot = 180f;
            transform.rotation = Quaternion.Euler(0f, playerRot, 0f);
        }


        //print($"isFlying: {isFlying}, verticalVelocity: {verticalVelocity}");
    }

    // verticalVelocity 계산: 정지 상태, 상승 중, 하강 중 일때 처리 구분
    private float CalculateVerticalVelocity()
    {
        float curYVelocity = rb.velocity.y;

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
        return isFlying || curYVelocity > 0f ? verticalSpeed : curYVelocity;
    }

    // 클릭한 블록 파괴 처리
    private void HandleDigging()
    {
        GameObject targetBlock = GetVaildBlockUnderMouse();

        if(!input.Player.Digging.IsPressed() || targetBlock == null)
            return;

        Block block = targetBlock.GetComponent<Block>();
        block.BlockDestroy(Time.deltaTime * 10f, playerScript);
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

        // 일반적인 캐기 거리
        float distance = Mathf.Sqrt(Mathf.Pow(hitPos.x - playerPos.x, 2) + Mathf.Pow(hitPos.y - playerPos.y, 2));

        // 캐릭터 기준 블록을 캘 수 있는 최대 거리
        float digDistance = IsStuckInSand() ? 
            NarrowDigDistance :     // 모래만 캘 수 있는 범위(0.5)
            (1.5f / transform.localScale.x) + maxHangDistance;

        return distance < digDistance ? hit.collider.gameObject : null;

        //print($"block Distance: {DigDistance}, Character Distance: {distance}");
    }

    private bool IsStuckInSand()
    {
        // 머리 위 오버랩 감지
        Vector2 headCheckPosition = (Vector2)transform.position + new Vector2(0, HeadCheckRadius);

        // 캐릭터도 감지되기 때문에 Layer를 Block으로 해 놓아야 함.
        Collider2D hit = Physics2D.OverlapCircle(headCheckPosition, HeadCheckRadius, blockLayer);

        if(hit == null)
            return false;

        Block block = hit.gameObject.GetComponent<Block>();
        if(block == null || block.blocksDictionary == null)
            return false;

        bool foundBlock = false;
        if(block.blockType == 6)
        {
            foundBlock = block.blocksDictionary.blockPosition.TryGetValue(block.transform.position, out GameObject _);
            if (foundBlock)
            {
                Debug.Log($"모래 블록 끼임 감지: {hit.name}");

            }
        }

        return foundBlock;

    }
}