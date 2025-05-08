using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory Inventory;
    [SerializeField] private List<Item> items;

    [SerializeField] private GameObject Inventory_obj;
    Vector3 Inventory_StartPos;
    Vector3 Inventory_EndPos;
    float currentTime = 0f;
    float moveTime = 1f;
    private bool isInventoryMoving = false;
    private bool isOnInventory = false;

    private void Awake()
    {
        Inventory_obj.transform.position = new Vector3(-200f, Screen.height / 2, 0f);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Inventory_StartPos = new Vector3(-200f, Screen.height / 2, 0f);
        Inventory_EndPos = new Vector3(0f, Screen.height / 2, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Inventory.AddItem(items[3], 1);
        //}

        // 인벤토리 Tab 액션
        if (Input.GetKeyDown(KeyCode.Tab) && !isInventoryMoving)
        {
            currentTime = 0f;
            isInventoryMoving = true;
        }
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
        else if(isInventoryMoving && isOnInventory)
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


        if (Input.GetKeyDown(KeyCode.F))
        {
            Inventory.ClearItem();
        }
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            Inventory.AddItem(items[0], 1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Inventory.AddItem(items[1], 1);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Inventory.SellItem(items[0]);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Inventory.SellItem(items[1]);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Inventory.SellItem(items[2]);
        }
    }
}
