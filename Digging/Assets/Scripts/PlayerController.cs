using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.Interactions;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour
{
    private PlayerControl playercontrols;

    private Vector2 moveInput;
    private float jumpInput;

    [SerializeField] private float moveSpeed = 2f;
    // 점프 관련 변수
    public float jumpSpeed = 5f;      // 점프 속도 (초기 속도)
    public float jumpAcceleration = 1f; // 점프 가속도 (점프 중 상승 속도)
    public float gravity = -9.81f;    // 중력 (아래로 가는 힘)

    private float currentJumpSpeed = 0f; // 현재 점프 속도
    [SerializeField] private float maxHangDistance = 0.5f;

    // 이전 outline 저장하는 변수
    static GameObject previousOutline = null;
    // Player 스크립트 인벤토리
    Player playerScript;


    bool isJumping = false;
    bool isdigging = false;

    private Rigidbody2D rb;
    private Collider2D characterCollider;

    private void Awake()
    {
        playercontrols = new PlayerControl();
        rb = GetComponent<Rigidbody2D>();
        characterCollider = GetComponent<CapsuleCollider2D>();
        playerScript = GetComponent<Player>();
    }

    private void OnEnable() => playercontrols.Enable();
    private void OnDisable() => playercontrols.Disable();

    private void Start()
    {
        playercontrols.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playercontrols.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        playercontrols.Player.Jump.started += ctx => jumpInput = ctx.ReadValue<float>();
        playercontrols.Player.Jump.started += ctx => isJumping = true;

        playercontrols.Player.Jump.canceled += ctx => jumpInput = 0;
        playercontrols.Player.Jump.canceled += ctx => isJumping = false;
    }

    private void Update()
    {
        // 클릭한 블록 저장
        GameObject block = GetVaildBlockUnderMouse();

        // 마우스 왼쪽 버튼 누르는 동안
        if (playercontrols.Player.Digging.IsPressed() && block != null)
        {
            block.GetComponent<Block>().BlockDestroy(Time.deltaTime, playerScript);
        }

        // 3. 블록 아웃라인 생성
        try
        {
            // 새로운 outline 가져오기
            GameObject outline = block?.transform.Find("outline")?.gameObject;

            // 이전 outline이 존재하고, 현재 block의 outline과 다르면 이전 것을 비활성화
            if(previousOutline != null && previousOutline != outline)
            {
                previousOutline.SetActive(false);
            }

            // 현재 block의 outline이 유효하면 활성화
            if (outline != null)
            {
                outline.SetActive(true);
                previousOutline = outline; // 새로운 outline을 기록
            }
            else
            {
                previousOutline = null; // outline이 없으면 기록을 null로
            }
        }
        catch
        {
            Debug.Log("블록이 없거나 outline이 없습니다");
        }
    }

    // 클릭한 블록 가져오기
    private GameObject GetVaildBlockUnderMouse()
    {
        // 마우스 레이저
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        // 콜라이더가 없(배경, 빈 화면)거나 블록인 경우만 캐기 
        if(hit.collider != null && hit.collider.CompareTag("Block"))
        {
            Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 hitPos = new Vector2(hit.transform.position.x, hit.transform.position.y);

            // 두 Vector의 사이의 거리
            float distance = Mathf.Sqrt(Mathf.Pow(hitPos.x - playerPos.x, 2) + Mathf.Pow(hitPos.y - playerPos.y, 2));
            // 캐릭터 기준 블록을 캘 수 있는 최대 거리
            float DigDistance = (1.5f / transform.localScale.x) + maxHangDistance;

            if(distance < DigDistance)
            {
                return hit.collider.gameObject;
            }
            //print($"block Distance: {DigDistance}, Character Distance: {distance}");
        }

        return null;
    }

    private void FixedUpdate()
    {
        float horizontal = moveInput.x * moveSpeed;
        //float vertical = jumpInput * jumpSpeed;

        //if(isJumping)
        //    rb.velocity = new Vector2(horizontal, vertical);
        //else // 나중에 가속도 추가 => 속도 변화량 * 걸린 시간
        //    rb.velocity = new Vector2(horizontal, -jumpSpeed);

        // 점프 가속 제로 추가와 보안해야 됨.
        if (jumpInput > 0 && !isJumping) { isJumping = true; currentJumpSpeed = jumpSpeed; }  // 점프 시작
        if (isJumping) { currentJumpSpeed += jumpAcceleration * Time.deltaTime; rb.velocity = new Vector2(horizontal, currentJumpSpeed); }
        else { rb.velocity = new Vector2(horizontal, gravity); }  // 중력 적용
        if (rb.velocity.y <= 0 && isJumping) { isJumping = false; currentJumpSpeed = 0f; }  // 점프 종료
    }
    // Tip: 코드 퍼스트, 모델 퍼스트, 데이터 퍼스트

    // TOOD: 플레이어가 모래에 갇혔을 때, 플레이어의 캐는 범위를 블럭 1로 설정을 한다. ( 시야 범위는 그냥 그대로 둬도 됨)
    // https://discussions.unity.com/t/new-input-system-how-to-use-the-hold-interaction/726823/4
    // 일단 Digging을 할 때 계속 꾹 누르게 된다고 한다면 계속 블럭을 캘 수 있게 하기 (마크처럼)
    //private IEnumerator DiggingProcess(InputAction.CallbackContext callback)
    //{
    //    isdigging = true;

    //    while(isdigging)
    //    {
    //        Digging();

    //        yield return isdigging = false;
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        Vector2 pos = contact.point;
        Vector2 normal = contact.normal;

        Debug.DrawRay(pos, normal, Color.red);

        if(collision.gameObject.tag == "Item")
        {
            Destroy(collision.gameObject);
        }
    }
}