using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Inventory Inventory;

    public Collection Collection;

    public List<Item> items;
    public List<Item> minerals;
    public List<Item> UseItems;
    public List<Item> UpgradeItems;

    // 인벤토리
    [SerializeField] private GameObject Inventory_obj;
    Vector3 Inventory_StartPos;
    Vector3 Inventory_EndPos;
    float currentTime = 0f;
    float moveTime = 1f;
    private bool isInventoryMoving = false;
    private bool isOnInventory = false;


    // 상점
    [SerializeField] private GameObject shopUIPanel; // 상점 UI 패널
    private bool isNearShop = false; // 상점 근처에 있는지 체크

    Vector3 Shop_StartPos;
    Vector3 Shop_EndPos;
    private bool isShopMoving = false;
    private bool isOnShop = false;

    // 박물관
    [SerializeField] private GameObject CollectUIPanel; // 도감 UI 패널
    private bool isNearMuseum = false;
    private bool isInMuseum = false;
    private bool isNearCollection = false;

    Vector3 Collect_StartPos;
    Vector3 Collect_EndPos;
    private bool isCollectMoving = false;
    private bool isOnCollect = false;

    // 진열장
    [SerializeField] private GameObject RelicInfoUIPanel; // 진열장 설명 UI 패널
    [SerializeField] private GameObject RelicInfoImage;
    [SerializeField] private GameObject RelicInfoName;
    [SerializeField] private GameObject RelicInfoText;


    private void Awake()
    {
        Inventory_obj.transform.position = new Vector3(-200f, Screen.height / 2, 0f);

        // 아이템 초기화
        for (int i = 0; i < items.Count; i++)
        {
            items[i].count = 0;
            items[i].accumulation_count = 0;
            items[i].ishaveitem = false;
            items[i].isalreadySell = false;

        }

        for (int i = 0; i < minerals.Count; i++)
        {
            minerals[i].count = 0;
        }

        for(int i = 0; i < UseItems.Count; i++)
        {
            UseItems[i].count = 0;
        }

        for(int i = 0; i < UpgradeItems.Count; i++)
        {
            UpgradeItems[i].count = 1;
            UpgradeItems[i].value = 10;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // UI 시작/도착 포지션 지정
        Inventory_StartPos = new Vector3(-200f, Screen.height / 2, 0f);
        Inventory_EndPos = new Vector3(0f, Screen.height / 2, 0f);

        Shop_StartPos = new Vector3(2420f, Screen.height / 2, 0f);
        Shop_EndPos = new Vector3(1920f, Screen.height / 2, 0f);

        Collect_StartPos = new Vector3(2420f, Screen.height / 2, 0f);
        Collect_EndPos = new Vector3(1920f, Screen.height / 2, 0f);

        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        Interaction_Inventory();
        Interaction_F();
        


        if (Input.GetKeyDown(KeyCode.G))
        {
            Inventory.ClearItem();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Inventory.AddItem(minerals[0], 1);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Inventory.AddItem(minerals[3], 1);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Inventory.AddItem(items[Random.Range(0,20)], 1);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Inventory.AddItem(items[5], 1);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Inventory.AddItem(items[7], 1);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Collection.collect_sum += 10;
        }
    }

    // 인벤토리 상호작용
    private void Interaction_Inventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isInventoryMoving && !isOnShop && !isOnCollect)
        {
            currentTime = 0f;
            isInventoryMoving = true;
        }

        // 인벤토리 UI 이동 애니메이션
        if (isInventoryMoving && !isOnInventory)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / moveTime);
            Inventory_obj.transform.position = Vector3.Lerp(Inventory_StartPos, Inventory_EndPos, t);

            if (t >= moveTime)
            {
                isInventoryMoving = false; // 애니메이션 완료
                isOnInventory = true;
            }
        }
        else if (isInventoryMoving && isOnInventory)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / moveTime);
            Inventory_obj.transform.position = Vector3.Lerp(Inventory_EndPos, Inventory_StartPos, t);

            if (t >= moveTime)
            {
                isInventoryMoving = false; // 애니메이션 완료
                isOnInventory = false;
            }
        }
    }

    // F 상호작용
    private void Interaction_F()
    {
        
        // 상점 UI 이동 애니메이션
        if (isShopMoving && !isOnShop)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / moveTime);
            shopUIPanel.transform.position = Vector3.Lerp(Shop_StartPos, Shop_EndPos, t);

            if (t >= moveTime)
            {
                isShopMoving = false; // 애니메이션 완료
                isOnShop = true;
            }
        }
        else if (isShopMoving && isOnShop)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / moveTime);
            shopUIPanel.transform.position = Vector3.Lerp(Shop_EndPos, Shop_StartPos, t);

            if (t >= moveTime)
            {
                isShopMoving = false; // 애니메이션 완료
                isOnShop = false;
            }
        }

        // 상점 상호작용
        if (isNearShop && Input.GetKeyDown(KeyCode.F) && !isInventoryMoving && !isShopMoving)
        {
            currentTime = 0f;

            if (isOnInventory && !isOnShop)
            {
                isShopMoving = true;

            }
            else
            {
                isShopMoving = true;
                isInventoryMoving = true;
            }


        }
        else if (!isNearShop && isOnShop && !isInventoryMoving && !isShopMoving)
        {
            currentTime = 0f;
            isInventoryMoving = true;
            isShopMoving = true;

        }

        // 박물관 상호작용
        if (isNearMuseum && Input.GetKeyDown(KeyCode.F))
        {

            if (!isInMuseum)
            {
                gameObject.transform.position = new Vector3(50f, 0.5f, 0f);
                isInMuseum = true;
            }
            else if (isInMuseum)
            {
                gameObject.transform.position = new Vector3(18f, 0.5f, 0f);
                isInMuseum = false;
            }

        }

        // 도감 상호작용

        // 도감 UI 이동 애니메이션
        if (isCollectMoving && !isOnCollect)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / moveTime);
            CollectUIPanel.transform.position = Vector3.Lerp(Collect_StartPos, Collect_EndPos, t);

            if (t >= moveTime)
            {
                isCollectMoving = false; // 애니메이션 완료
                isOnCollect = true;
            }
        }
        else if (isCollectMoving && isOnCollect)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / moveTime);
            CollectUIPanel.transform.position = Vector3.Lerp(Collect_EndPos, Collect_StartPos, t);

            if (t >= moveTime)
            {
                isCollectMoving = false; // 애니메이션 완료
                isOnCollect = false;
            }
        }

        // 도감 상호작용
        if (isNearCollection && Input.GetKeyDown(KeyCode.F) && !isInventoryMoving && !isCollectMoving)
        {
            currentTime = 0f;

            if (isOnInventory && !isOnCollect)
            {
                isCollectMoving = true;

            }
            else
            {
                isCollectMoving = true;
                isInventoryMoving = true;
            }

            // 획득한 유물을 도감에 활성화
            for(int i = 0; i < items.Count; i++)
            {
                Collection.li_isCollect[i] = items[i].ishaveitem;
                if(Collection.li_isCollect[i] == true)
                {
                    Collection.slots[i].item = items[i];
                }
            }
            

        }
        else if (!isNearCollection && isOnCollect && !isInventoryMoving && !isCollectMoving)
        {
            currentTime = 0f;
            isInventoryMoving = true;
            isCollectMoving = true;

        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shop"))
        {
            isNearShop = true;
            Debug.Log("상점에 접근했습니다. F 키로 상호작용.");
        }
        if (other.CompareTag("Museum"))
        {
            isNearMuseum = true;
            Debug.Log("박물관에 접근했습니다. F 키로 상호작용.");
        }
        if (other.CompareTag("Collect"))
        {
            isNearCollection = true;
            Debug.Log("도감에 접근했습니다. F 키로 상호작용.");
        }
        if (other.CompareTag("Table"))
        {
            Debug.Log(other.gameObject.name);
            int idx = int.Parse(other.gameObject.name);
            if (Collection.li_isRelicOnTable[idx] == true)
            {
                RelicInfoImage.GetComponent<Image>().sprite = items[idx].itemImage;
                RelicInfoName.GetComponent<TextMeshProUGUI>().text = items[idx].itemName;
                RelicInfoText.GetComponent<TextMeshProUGUI>().text = items[idx].Info;
                RelicInfoUIPanel.SetActive(true);
            }

            
            Debug.Log("진열장에 접근했습니다.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Shop"))
        {
            isNearShop = false;
            Debug.Log("상점에서 벗어났습니다.");
        }
        if (other.CompareTag("Museum"))
        {
            isNearMuseum = false;
            Debug.Log("박물관에서 벗어났습니다.");
        }
        if (other.CompareTag("Collect"))
        {
            isNearCollection = false;
            Debug.Log("도감에서 벗어났습니다.");
        }
        if (other.CompareTag("Table"))
        {
            RelicInfoUIPanel.SetActive(false);
            Debug.Log("진열장에서 벗어났습니다.");
        }
    }

}
