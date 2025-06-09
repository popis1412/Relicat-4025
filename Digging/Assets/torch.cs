using UnityEngine;
using UnityEngine.Rendering.Universal;


public class torch : MonoBehaviour
{
    [SerializeField] GameObject dropItem;
    [SerializeField] Player playerScript;
    Light2D light2D;
    float lightInnerRadius = 0.3f;
    float randFloat;

    bool itemDropped = false;

    private void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        light2D = this.gameObject.GetComponent<Light2D>();
        playerScript = playerObj.GetComponent<Player>();
        randFloat = Random.Range(0f, 1f);
    }

    private void Update()
    {
        CheckBelowAndDrop();

        if(light2D != null)
        {
            float radiusPingpong1 = Mathf.PingPong(Time.time / 3 + randFloat, 0.5f);
            light2D.pointLightInnerRadius = radiusPingpong1;
        }
    }

    public void CheckBelowAndDrop()
    {
        if(itemDropped) return;

        Vector2 origin = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 0.5f;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, LayerMask.GetMask("Block"));

        if(hit.collider == null)
        {
            ItemDrop(2, 1, playerScript, 1);
            Destroy(gameObject);
            itemDropped = true;
        }
        else
        {
            itemDropped = false;
        }

    }

    void ItemDrop(int itemType, int itemCode, Player playerScript, int addEA) //itemType 0�� ����, 1�� ����
    {
        GameObject newDropItem = Instantiate(dropItem, this.transform.position, Quaternion.identity);

        DropItem dropItemScript = newDropItem.GetComponent<DropItem>();

        Sprite dropItemSprite = null;

        if(itemType == 2)
        {
            dropItemSprite = playerScript.UseItems[itemCode].itemImage;
        }

        dropItemScript.setDropItem(itemType, itemCode, dropItemSprite, addEA);
    }
}
