using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] Player playerScript;
    [SerializeField] GameObject effect;

    Vector3 originalScale;
    float pulseDuration = 0.5f;
    float explosionDelay = 3f;

    float exploSize = 2.5f;

    float elapsed = 0f;

    void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerObj.GetComponent<Player>();
        // ��ġ�ϱ� ���� �浹 ���� ��Ȱ��ȭ
        HandleIgnoreCollision();
    }

    void Start()
    {
        originalScale = transform.localScale;
        StartCoroutine(ExplosionRoutine()); // ���� ī��Ʈ�ٿ� ����
    }

    void Update()
    {
        HandleIgnoreCollision();
        HandlePulsingScale();
    }

    void HandlePulsingScale()
    {
        elapsed += Time.deltaTime;

        float t = Mathf.PingPong(elapsed, pulseDuration) / pulseDuration;
        float scale = Mathf.Lerp(1f, 1.5f, t);
        transform.localScale = originalScale * scale;
    }

    void HandleIgnoreCollision()
    {
        Collider2D bombCol = GetComponent<Collider2D>();
        if(bombCol == null) return;

        if (playerScript != null)
        {
            Collider2D playerCol = playerScript.GetComponent<Collider2D>();
            if(playerCol != null)
                Physics2D.IgnoreCollision(bombCol, playerCol, true);
        }

        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
        foreach(GameObject bomb in bombs)
        {
            foreach (Collider2D col in bomb.GetComponents<Collider2D>())
            {
                Physics2D.IgnoreCollision(bombCol, col, true);
            }
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            foreach(Collider2D col in enemy.GetComponents<Collider2D>())
            {
                Physics2D.IgnoreCollision(bombCol, col, true);
            }
        }
    }

    IEnumerator ExplosionRoutine()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
        Destroy(gameObject);
    }

    void Explode()
    {
        // ���� ������� ���⿡�� ���ϸ� ��.
        Vector2 explosionSize = new Vector2(1f * exploSize, 0.5f);

        //DrawDebugBox(transform.position, explosionSize, Color.red);

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, explosionSize, 0f);

        foreach(Collider2D hit in hits)
        {
            if(hit.CompareTag("Block"))
            {
                Block block = hit.GetComponent<Block>();
                if(block != null && block.blockType != -1 || block.blockType != 1)
                {
                    block.BlockDestroy(10, playerScript);
                }
            }

            if(hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if(enemy != null)
                    Destroy(enemy.gameObject);
            }

            if(hit.CompareTag("Player"))
            {
                PlayerController player = hit.GetComponent<PlayerController>();
                if(player != null)
                    player.TakeDamage(1, transform);
            }

            if(hit.gameObject.name == "torch")
            {
                torch torch = hit.GetComponent<torch>();
                if(torch != null)
                {
                    torch.CheckBelowAndDrop();
                }
                    
            }
        }
    }

    private void OnDestroy()
    {
        Instantiate(effect, transform.position, Quaternion.identity);
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
}
