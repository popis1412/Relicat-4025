using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory Inventory;
    public Collection Collection;
    public List<Item> items;
    public List<Item> minerals;

    // �κ��丮
    [SerializeField] private GameObject Inventory_obj;
    Vector3 Inventory_StartPos;
    Vector3 Inventory_EndPos;
    float currentTime = 0f;
    float moveTime = 1f;
    private bool isInventoryMoving = false;
    private bool isOnInventory = false;


    // ����
    [SerializeField] private GameObject shopUIPanel; // ���� UI �г�
    private bool isNearShop = false; // ���� ��ó�� �ִ��� üũ

    Vector3 Shop_StartPos;
    Vector3 Shop_EndPos;
    private bool isShopMoving = false;
    private bool isOnShop = false;

    // �ڹ���
    [SerializeField] private GameObject CollectUIPanel; // ���� UI �г�
    private bool isNearMuseum = false;
    private bool isInMuseum = false;
    private bool isNearCollection = false;

    Vector3 Collect_StartPos;
    Vector3 Collect_EndPos;
    private bool isCollectMoving = false;
    private bool isOnCollect = false;


    private void Awake()
    {
        Inventory_obj.transform.position = new Vector3(-200f, Screen.height / 2, 0f);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // UI ����/���� ������ ����
        Inventory_StartPos = new Vector3(-200f, Screen.height / 2, 0f);
        Inventory_EndPos = new Vector3(0f, Screen.height / 2, 0f);

        Shop_StartPos = new Vector3(2420f, Screen.height / 2, 0f);
        Shop_EndPos = new Vector3(1920f, Screen.height / 2, 0f);

        Collect_StartPos = new Vector3(2420f, Screen.height / 2, 0f);
        Collect_EndPos = new Vector3(1920f, Screen.height / 2, 0f);

        // ������ �ʱ�ȭ
        for (int i = 0; i < items.Count; i++)
        {
            items[i].count = 0;
            items[i].ishaveitem = false;
            items[i].isalreadySell = false;
        }

        for(int i = 0; i < minerals.Count; i++)
        {
            minerals[i].count = 0;
        }
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
            Inventory.AddItem(items[0], 1);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Inventory.AddItem(items[5], 1);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Inventory.AddItem(items[7], 1);
        }

    }

    // �κ��丮 ��ȣ�ۿ�
    private void Interaction_Inventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isInventoryMoving && !isOnShop)
        {
            currentTime = 0f;
            isInventoryMoving = true;
        }

        // �κ��丮 UI �̵� �ִϸ��̼�
        if (isInventoryMoving && !isOnInventory)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / moveTime);
            Inventory_obj.transform.position = Vector3.Lerp(Inventory_StartPos, Inventory_EndPos, t);

            if (t >= moveTime)
            {
                isInventoryMoving = false; // �ִϸ��̼� �Ϸ�
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
                isInventoryMoving = false; // �ִϸ��̼� �Ϸ�
                isOnInventory = false;
            }
        }
    }

    // F ��ȣ�ۿ�
    private void Interaction_F()
    {
        
        // ���� UI �̵� �ִϸ��̼�
        if (isShopMoving && !isOnShop)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / moveTime);
            shopUIPanel.transform.position = Vector3.Lerp(Shop_StartPos, Shop_EndPos, t);

            if (t >= moveTime)
            {
                isShopMoving = false; // �ִϸ��̼� �Ϸ�
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
                isShopMoving = false; // �ִϸ��̼� �Ϸ�
                isOnShop = false;
            }
        }

        // ���� ��ȣ�ۿ�
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

        // �ڹ��� ��ȣ�ۿ�
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

        // ���� ��ȣ�ۿ�

        // ���� UI �̵� �ִϸ��̼�
        if (isCollectMoving && !isOnCollect)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / moveTime);
            CollectUIPanel.transform.position = Vector3.Lerp(Collect_StartPos, Collect_EndPos, t);

            if (t >= moveTime)
            {
                isCollectMoving = false; // �ִϸ��̼� �Ϸ�
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
                isCollectMoving = false; // �ִϸ��̼� �Ϸ�
                isOnCollect = false;
            }
        }

        // ���� ��ȣ�ۿ�
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

            // ȹ���� ������ ������ Ȱ��ȭ
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
            Debug.Log("������ �����߽��ϴ�. F Ű�� ��ȣ�ۿ�.");
        }
        if (other.CompareTag("Museum"))
        {
            isNearMuseum = true;
            Debug.Log("�ڹ����� �����߽��ϴ�. F Ű�� ��ȣ�ۿ�.");
        }
        if (other.CompareTag("Collect"))
        {
            isNearCollection = true;
            Debug.Log("������ �����߽��ϴ�. F Ű�� ��ȣ�ۿ�.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Shop"))
        {
            isNearShop = false;
            Debug.Log("�������� ������ϴ�.");
        }
        if (other.CompareTag("Museum"))
        {
            isNearMuseum = false;
            Debug.Log("�ڹ������� ������ϴ�.");
        }
        if (other.CompareTag("Collect"))
        {
            isNearCollection = false;
            Debug.Log("�������� ������ϴ�.");
        }
    }

}
