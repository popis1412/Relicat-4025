using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    private PlayerControl playercontrols;

    private Vector2 moveInput;
    private float jumpInput;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpSpeed = 5f;

    [SerializeField] private float timeDig = 0.5f;     // 블록 캐는 시간
    [SerializeField] private float startholdtime;    // 처음 누른 시간
    [SerializeField] private float lastDigTime;      // 마지막 캔 시점


    bool isJumping = false;
    bool isdigging = false;
    float fallSpeed = -5f;

    private Rigidbody2D rb;
    private Collider2D characterCollider;

    private void Awake()
    {
        playercontrols = new PlayerControl();
        rb = GetComponent<Rigidbody2D>();
        characterCollider = GetComponent<CapsuleCollider2D>();
    }

    private void OnEnable() => playercontrols.Enable();
    private void OnDisable() => playercontrols.Disable();

    private void Start()
    {
        lastDigTime = 0f;

        playercontrols.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playercontrols.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        playercontrols.Player.Jump.started += ctx => jumpInput = ctx.ReadValue<float>();
        playercontrols.Player.Jump.started += ctx => isJumping = true;

        playercontrols.Player.Jump.canceled += ctx => jumpInput = 0;
        playercontrols.Player.Jump.canceled += ctx => isJumping = false;

        playercontrols.Player.Digging.started += DiggingStart;
        playercontrols.Player.Digging.performed += Digging;
    }

    private void DiggingStart(InputAction.CallbackContext context)
    {
        startholdtime = Time.time;
    }

    private void Digging(InputAction.CallbackContext context)
    {
        // 마우스 레이저
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        // 콜라이더가 없(배경, 빈 화면)거나 블록인 경우만 캐기 
        if(hit.collider != null && hit.collider.CompareTag("Block"))
        {
            Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 hitPos = new Vector2(hit.transform.position.x, hit.transform.position.y);

            float distance = Mathf.Sqrt(Mathf.Pow(hitPos.x - playerPos.x, 2) +
                Mathf.Pow(hitPos.y - playerPos.y, 2));


            // 타일과 충돌 및 두 물체의 거리가 √2보다 작으면 블록을 캐기
            // 0.2f를 더한 이유는 플레이어가 블럭으로부터 항상 가운데(0.5)에 있는 것이 아니기 때문에
            // 플레이어가 아슬하게 걸치는 거리인 0.2를 더한 것임. 
            if(distance < (1.5f / transform.localScale.x) + 0.2f)
            {
                GameObject click_obj = hit.transform.gameObject;
                if(Time.time > startholdtime + timeDig)
                {
                    lastDigTime = startholdtime;
                    click_obj.GetComponent<Block>().BlockDestroy(lastDigTime);
                }
            }

            //print(distance);
        }
    }

    private void FixedUpdate()
    {
        float horizontal = moveInput.x * moveSpeed;
        float vertical = jumpInput * jumpSpeed;

        if(isJumping)
            rb.velocity = new Vector2(horizontal, vertical);
        else // 나중에 가속도 추가 => 속도 변화량 * 걸린 시간
            rb.velocity = new Vector2(horizontal, -jumpSpeed);
    }
    // Tip: 코드 퍼스트, 모델 퍼스트, 데이터 퍼스트

    // TOOD: 플레이어가 모래에 갇혔을 때, 플레이어의 캐는 범위를 블럭 1로 설정을 한다. ( 시야 범위는 그냥 그대로 둬도 됨)
    void Digging()
    {
        
    }
    // https://discussions.unity.com/t/new-input-system-how-to-use-the-hold-interaction/726823/4
    // 일단 Digging을 할 때 계속 꾹 누르게 된다고 한다면 계속 블럭을 캘 수 있게 하기 (마크처럼)
    private IEnumerator DiggingProcess(InputAction.CallbackContext callback)
    {
        isdigging = true;

        while(isdigging)
        {
            Digging();

            yield return isdigging = false;
        }
    }

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