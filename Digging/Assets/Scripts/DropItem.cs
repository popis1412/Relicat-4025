using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DropItem : MonoBehaviour
{
    public int itemType; //0�̸� ����, 1�̸� ����
    public int itemCode; //������ ����
    public int addEA; //������ ����
    Rigidbody2D rigidbody;

    float count = 0;
    bool canTake = false;

    Light2D light2D;
    float lightInnerRadius = 0.1f;
    float lightOuterRadius = 0.3f;


    void Awake()
    {
        rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        light2D = this.gameObject.GetComponent<Light2D>();
    }
    void Start()
    {
        print("����");
        float x = Random.Range(-1, 1);
        float y = Random.Range(2, 3);
        rigidbody.velocity = new Vector2(x, y);
    }

    // Update is called once per frame
    void Update()
    {
        if (count < 0.6 && canTake == false)
        {
            count += Time.deltaTime;
        }
        else
        {
            canTake = true;
        }

        if (light2D != null)
        {
            float radiusPingpong = Mathf.PingPong(Time.time / 5f, 0.15f);
            light2D.pointLightInnerRadius = lightInnerRadius + radiusPingpong;
            //light2D.pointLightOuterRadius = lightOuterRadius + radiusPingpong;
        }


    }
    public void setDropItem(int newItemType, int newItemCode, Sprite renderSprite, int newAddEA)
    {
        itemType = newItemType;
        itemCode = newItemCode;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = renderSprite;
        addEA = newAddEA;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), collision.GetComponent<CapsuleCollider2D>(), true);

        if (collision.tag == "Player" && canTake)
        {
            Player playerScript = collision.GetComponent<Player>();

            if (playerScript != null)
            {
                if(itemType == 0) //�� �������� �����̸�
                {
                    playerScript.Inventory.AddItem(playerScript.items[itemCode], addEA);
                }
                else if (itemType == 1) //�� �������� �����̸�
                {
                    playerScript.Inventory.AddItem(playerScript.minerals[itemCode], addEA);
                }
                else if(itemType == 2) // �� �������� ��� �������̶��
                {
                    playerScript.Inventory.AddItem(playerScript.UseItems[itemCode], addEA);
                }
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), collision.GetComponent<CapsuleCollider2D>(), true);

        if (collision.tag == "Player" && canTake)
        {
            Player playerScript = collision.GetComponent<Player>();

            if (playerScript != null)
            {
                if (itemType == 0) //�� �������� �����̸�
                {
                    playerScript.Inventory.AddItem(playerScript.items[itemCode], addEA);
                }
                else if (itemType == 1) //�� �������� �����̸�
                {
                    playerScript.Inventory.AddItem(playerScript.minerals[itemCode], addEA);
                }
                else if(itemType == 2) // �� �������� ��� �������̶��
                {
                    playerScript.Inventory.AddItem(playerScript.UseItems[itemCode], addEA);
                }
               Destroy(this.gameObject);
            }
        }
    }
}
