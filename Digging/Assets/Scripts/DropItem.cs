using Spine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class DropItem : MonoBehaviour
{
    public int itemType; //0이면 유물, 1이면 광물
    public int itemCode; //아이템 종류
    public int addEA; //아이템 개수
    Rigidbody2D rigidbody;

    [SerializeField] GameObject relicEffect;

    // 액션
    public System.Action<SlotInfo, Item> OnItemPicked;   // 아이템 먹음

    float count = 0;
    bool canTake = false;

    Light2D light2D;
    float lightInnerRadius = 0.1f;
    float lightOuterRadius = 0.3f;

    private bool tryOnce = false;


    void Awake()
    {
        rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        light2D = this.gameObject.GetComponent<Light2D>();
    }

    void Start()
    {
        float x = Random.Range(-1, 1);
        float y = Random.Range(2, 3);
        rigidbody.velocity = new Vector2(x, y);

        if (itemType == 0)
        {
            relicEffect.SetActive(true);
        }

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
                if(itemType == 0) //이 아이템이 유물이면
                {
                    playerScript.Inventory.AddItem(playerScript.items[itemCode], addEA);
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[15]);
                }
                else if (itemType == 1) //이 아이템이 보석이면
                {
                    playerScript.Inventory.AddItem(playerScript.minerals[itemCode], addEA);
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[13]);
                }
                else if(itemType == 2) // 이 아이템이 사용 아이템이라면
                {
                    playerScript.Inventory.AddItem(playerScript.UseItems[itemCode], addEA);

                    switch(playerScript.UseItems[itemCode].type)
                    {
                        case ItemType.Bomb:
                        case ItemType.Torch:
                        case ItemType.Teleport:
                            SlotManager.Instance.ItemPick(playerScript.UseItems[itemCode], addEA); // 먹은 아이템 보내기
                            break;
                        default:
                            break;
                    }


                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[13]);
                }
                else if(itemType == 3) // 이 아이템이 드릴 아이템이라면
                {
                    playerScript.Inventory.AddItem(playerScript.Drill_Items[itemCode], addEA);
                    
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[13]);
                    Inventory.instance.LogMessage("드릴 부품을 획득했습니다.");
                }
                else if (itemType == 4) // 이 아이템이 드릴 배터리 아이템이라면
                {
                    if (playerScript.Drill_Items[itemCode] == playerScript.Drill_Items[4])
                    {
                        // 드릴이라는 아이템이 퀵슬롯에 있다면 배터리 채우기
                        if(SlotManager.Instance.FindSlot(WeaponType.Drill))
                        {
                            Inventory.instance.LogMessage("드릴의 배터리가 충전되었습니다.");
                            SlotManager.Instance.PickupEnergy?.Invoke();
                            SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[13]);
                        }
                        else
                        {
                            print("드릴이라는 아이템이 없습니다.");
                            return;
                        }
                    }
                }

                Destroy(this.gameObject);

                Debug.Log("enter");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), collision.GetComponent<CapsuleCollider2D>(), true);

        if (collision.tag == "Player" && canTake && tryOnce == false)
        {
            Player playerScript = collision.GetComponent<Player>();

            if (playerScript != null)
            {
                if (itemType == 0) //이 아이템이 유물이면
                {
                    playerScript.Inventory.AddItem(playerScript.items[itemCode], addEA);
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[15]);
                }
                else if (itemType == 1) //이 아이템이 보석이면
                {
                    playerScript.Inventory.AddItem(playerScript.minerals[itemCode], addEA);
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[13]);
                }
                else if (itemType == 2) // 이 아이템이 사용 아이템이라면
                {
                    playerScript.Inventory.AddItem(playerScript.UseItems[itemCode], addEA);

                    switch(playerScript.UseItems[itemCode].type)
                    {
                        case ItemType.Bomb:
                        case ItemType.Torch:
                        case ItemType.Teleport:
                            SlotManager.Instance.ItemPick(playerScript.UseItems[itemCode], addEA); // 먹은 아이템 보내기
                            break;
                        default:
                            break;
                    }

                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[13]);
                }
                else if (itemType == 3) // 이 아이템이 드릴 아이템이라면
                {
                    playerScript.Inventory.AddItem(playerScript.Drill_Items[itemCode], addEA);
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[13]);
                    Inventory.instance.LogMessage("드릴 부품을 획득했습니다.");
                }
                else if (itemType == 4) // 이 아이템이 드릴 배터리 아이템이라면
                {
                    if (playerScript.Drill_Items[itemCode] == playerScript.Drill_Items[4])
                    {
                        // 드릴이라는 아이템이 퀵슬롯에 있다면 배터리 채우기
                        if(SlotManager.Instance.FindSlot(WeaponType.Drill))
                        {
                            Inventory.instance.LogMessage("드릴의 배터리가 충전되었습니다.");
                            SlotManager.Instance.PickupEnergy?.Invoke();
                            SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[13]);
                        }
                        else
                        {
                            print("드릴이라는 아이템이 없습니다.");
                            return;
                        }
                    }
                }
                tryOnce = true;
                Destroy(this.gameObject);
                
            }

            Debug.Log("stay");
        }
    }
}

