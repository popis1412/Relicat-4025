using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] Player playerScript;
    [SerializeField] private ItemInstance _instance;

    // 카운트
    float timer;
    float countdown = 4f;

    // 애니메이션
    Animator anim;
    bool isCountingDown = false;
    bool hasExploded = false;

    // 폭발 범위
    [SerializeField] Vector2 exploSize = new Vector2(3f, 1f);

    // 사이즈 조절
    Vector3 originalScale;

    float minScale = 0.5f;
    float maxScale = 1.5f;
    float pulseDuration = 1f; // 총 1초 주기

    // 폭탄 던지기
    private Rigidbody2D rb;

    float throwForceX = 5f;
    float throwForceY = 5f;

    private bool isGrounded = false;
    private bool isAlreadyDamaged = false;

    public void Setup(ItemInstance instance)
    {
        _instance = instance;
    }

    private void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    private void OnEnable()
    {
        StartCountdown();
    }

    void StartCountdown()
    {
        timer = countdown;
        isCountingDown = true;
    }

    void Start()
    {
        originalScale = Vector3.one;

        rb = GetComponent<Rigidbody2D>();

        // 생성 후, 던지기
        Throw(GetPlayerDirection());

        //originalScale = transform.localScale;
        //StartCoroutine(ExplosionRoutine()); // 폭발 카운트다운 시작
    }

    private void Update()
    {
        if(!isCountingDown || hasExploded) return;

        timer -= Time.deltaTime;

        if(timer <= 0f)
            Explode();

        // 시간 비율 계산 (0 ~ 1 ~ 0 반복)
        float t = Mathf.PingPong(Time.time / pulseDuration, 1f);
        float scale = Mathf.SmoothStep(minScale, maxScale, t);

        Vector3 newScale = originalScale * scale;
        transform.localScale = newScale;
    }

    Vector2 GetPlayerDirection()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
            return Vector2.right * throwForceX; // fallback

        Vector2 playerPos = player.transform.position;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouseWorldPos - playerPos).normalized;

        // fallback: 방향이 없다면 바라보는 방향
        if(dir == Vector2.zero)
            dir = new Vector2(Mathf.Sign(player.transform.localScale.x), 0f);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;

        // 8방향
        Vector2[] directions = {
            new Vector2(1, 0),    // 0: →
            new Vector2(1, 1),    // 1: ↗
            new Vector2(0, 1),    // 2: ↑
            new Vector2(-1, 1),   // 3: ↖
            new Vector2(-1, 0),   // 4: ←
            new Vector2(-1, -1),  // 5: ↙
            new Vector2(0, -1),   // 6: ↓
            new Vector2(1, -1)    // 7: ↘
        };

        int index = Mathf.RoundToInt(angle / 45f) % 8;
        Vector2 selectedDir = directions[index];

        float forceX = throwForceX;
        float forceY = throwForceY;

        switch(index)
        {
            case 0: // →
                forceX *= 1.5f;
                break;

            case 1: // ↗
                forceX *= 1.5f;
                forceY *= 1.5f;
                break;

            case 2: // ↑
                break;

            case 3: // ↖
                forceX *= 1.5f;
                forceY *= 1.5f;
                break;

            case 4: // ←
                forceX *= 1.5f;
                break;

            case 5: // ↙
                forceX *= 1.5f;
                forceY = 0;
                break;

            case 6: // ↓
                return Vector2.zero;

            case 7: // ↘
                forceX *= 1.5f;
                forceY = 0;
                break;
        }

        return new Vector2(selectedDir.x * forceX, selectedDir.y * forceY);
    }

    public void Throw(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    //void HandleIgnoreCollision()
    //{
    //    Collider2D bombCol = GetComponent<Collider2D>();
    //    if(bombCol == null) return;

    //    if (playerScript != null)
    //    {
    //        Collider2D playerCol = playerScript.GetComponent<Collider2D>();
    //        if(playerCol != null)
    //            Physics2D.IgnoreCollision(bombCol, playerCol, true);
    //    }

    //    GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
    //    foreach(GameObject bomb in bombs)
    //    {
    //        foreach (Collider2D col in bomb.GetComponents<Collider2D>())
    //        {
    //            Physics2D.IgnoreCollision(bombCol, col, true);
    //        }
    //    }

    //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //    foreach(GameObject enemy in enemies)
    //    {
    //        foreach(Collider2D col in enemy.GetComponents<Collider2D>())
    //        {
    //            Physics2D.IgnoreCollision(bombCol, col, true);
    //        }
    //    }
    //}

    //IEnumerator ExplosionRoutine()
    //{
    //    yield return new WaitForSeconds(explosionDelay);
    //    Explode();
    //    Destroy(gameObject);
    //}

    void Explode()
    {
        hasExploded = true;
        // 애니메이션 재생
        anim.enabled = true;

        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[19]);


        // 1. 폭발 크기 지정
        Vector2 explosionSize = new Vector2(1f * exploSize.x, 1f * exploSize.y);
        Vector2 explosionCenter = transform.position;
        Vector2 explosionHalfSize = explosionSize * 0.5f;

        DrawDebugBox(transform.position, explosionSize, Color.red);
        Debug.Break();

        // 3. 충돌 감지 (전체 대상)
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, explosionSize, 0f);

        // 4. 범위 내 중심만 필터링
        foreach(var hit in hits)
        {
            Vector2 hitCenter = hit.bounds.center;

            bool isInsideExplosion =
                Mathf.Abs(hitCenter.x - explosionCenter.x) <= explosionHalfSize.x &&
                Mathf.Abs(hitCenter.y - explosionCenter.y) <= explosionHalfSize.y;

            if(!isInsideExplosion)
                continue;


            string tag = hit.tag;

            switch(tag)
            {
                case "Block":
                    TryDestroyBlock(hit);
                    break;

                case "Enemy":
                    TryDestroyEnemy(hit);
                    break;

                case "Player":
                    TryDamagePlayer(hit);
                    break;

                case "Torch":
                    TryTriggerTorch(hit);
                    break;
            }
        }
    }

    void TryDestroyBlock(Collider2D hit)
    {
        Block block = hit.GetComponent<Block>();
        if(block != null && block.blockType != -1 && block.blockType != 1)
        {
            block.BlockDestroy(10, playerScript);
        }
    }

    void TryDestroyEnemy(Collider2D hit)
    {
        Enemy enemy = hit.GetComponent<Enemy>();
        if(enemy != null)
        {
            SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[25]);
            Destroy(enemy.gameObject);
        }
    }

    void TryDamagePlayer(Collider2D hit)
    {
        if(isAlreadyDamaged) return;

        PlayerController player = hit.GetComponent<PlayerController>();
        if(player != null)
        {
            player.TakeDamage(1, transform);
            isAlreadyDamaged = true;
        }
    }

    void TryTriggerTorch(Collider2D hit)
    {
        Torch torch = hit.GetComponent<Torch>();
        if(torch != null)
        {
            torch.CheckBelowAndDrop();
        }
    }

    public void OnExplosionFinished()
    {
        Destroy(gameObject);
    }


    void DrawDebugBox(Vector2 center, Vector2 size, Color color)
    {
        Vector2 halfSize = size * 0.5f;

        Vector2 topLeft = center + new Vector2(-halfSize.x, halfSize.y);
        Vector2 topRight = center + new Vector2(halfSize.x, halfSize.y);
        Vector2 bottomRight = center + new Vector2(halfSize.x, -halfSize.y);
        Vector2 bottomLeft = center + new Vector2(-halfSize.x, -halfSize.y);

        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topRight, bottomRight, color);
        Debug.DrawLine(bottomRight, bottomLeft, color);
        Debug.DrawLine(bottomLeft, topLeft, color);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.collider.CompareTag("Block")) return;

        ContactPoint2D contact = collision.GetContact(0);

        if(!isGrounded && IsGroundContact(contact.normal))
        {
            isGrounded = true;
            rb.velocity = Vector2.zero;
            Debug.Log("💥 바닥에 닿아 정지됨");
        }
    }

    bool IsGroundContact(Vector2 normal)
    {
        return normal.y > 0.5f;
    }
}
