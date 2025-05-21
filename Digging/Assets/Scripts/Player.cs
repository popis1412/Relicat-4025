using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory Inventory;
    public List<Item> items;
    public List<Item> minerals;

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


        if (Input.GetKeyDown(KeyCode.Z))
        {
            Inventory.AddItem(minerals[0], 1);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Inventory.AddItem(items[9], 1);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Inventory.AddItem(items[17], 1);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            Inventory.SellItem(minerals[0]);
            //Debug.Log(minerals[0].value);
            //Debug.Log(minerals[0].duplicate_value);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Inventory.SellItem(items[9]);
            //Debug.Log(items[9].value);
            //Debug.Log(items[9].duplicate_value);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Inventory.SellItem(items[17]);
            //Debug.Log(items[17].value);
            //Debug.Log(items[17].duplicate_value);
        }

    }
}
