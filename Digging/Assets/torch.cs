using UnityEngine;


public class torch : MonoBehaviour
{
    [SerializeField] GameObject dropItem;
    [SerializeField] Player playerScript;

    bool itemDropped = false;

    private void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerObj.GetComponent<Player>();
    }

    private void Update()
    {
        CheckBelowAndDrop();
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

    void ItemDrop(int itemType, int itemCode, Player playerScript, int addEA) //itemType 0은 유물, 1은 광물
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
