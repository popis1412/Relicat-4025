using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public List<Item> items;
    public int money;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;
    



#if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
        
    }
#endif

    void Awake()
    {
        FreshSlot();
    }

    public void FreshSlot()
    {
        int i = 0;
        for (; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];

            // UI 텍스트 업데이트
            Transform parent = slots[i].transform.parent;
            TextMeshProUGUI childtext = parent.GetChild(1).GetComponent<TextMeshProUGUI>();
            childtext.text = items[i].count.ToString();
        }

        for (; i < slots.Length; i++)
        {
            slots[i].item = null;

            // 빈 슬롯이면 텍스트 비우기
            Transform parent = slots[i].transform.parent;
            TextMeshProUGUI childtext = parent.GetChild(1).GetComponent<TextMeshProUGUI>();
            childtext.text = "";
        }
    }

    // addEA: 갯수, _item: Item Object
    public void AddItem(Item _item, int addEA)
    {

        foreach (Item item in items)
        {
            if (item.itemName == _item.itemName)
            {
                _item.count += addEA;
                for(int i = 0; i < items.Count; i++)
                {
                    if(slots[i].item.itemName == _item.itemName)
                    {
                        Transform parent = slots[i].transform.parent;
                        TextMeshProUGUI childtext = parent.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                        Debug.Log(childtext);
                        childtext.text = item.count.ToString();
                    }
                    
                }
                FreshSlot();
                return;
            }
        }

        // 새로운 아이템 추가 (슬롯 여유가 있을 경우)
        if (items.Count < slots.Length)
        {
            items.Add(_item);
            FreshSlot();
            _item.count += addEA;
            for (int i = 0; i < items.Count; i++)
            {
                if (slots[i].item.itemName == _item.itemName)
                {
                    Transform parent = slots[i].transform.parent;
                    TextMeshProUGUI childtext = parent.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                    Debug.Log(childtext);
                    childtext.text = _item.count.ToString();
                }
            }
        }
        else
        {
            Debug.Log("슬롯이 가득 차 있습니다.");
        }
    }
    public void ClearItem()
    {
        foreach (Item item in items)
        {
            item.count = 0;
        }
        for (int i = 0; i < items.Count; i++)
        {
            Transform parent = slots[i].transform.parent;
            TextMeshProUGUI childtext = parent.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            Debug.Log(childtext);
            childtext.text = "";
        }
        items.Clear();
        FreshSlot();

        money = 0;
        
    }

    public void SellItem(Item _item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == _item.itemName)
            {
                items[i].count--;

                if (items[i].count <= 0)
                {
                    items[i].count = 0;
                    items.RemoveAt(i);
                }

                money += _item.value;
                Debug.Log(money);
                break; // 아이템은 유일하다고 가정
            }
        }

        FreshSlot(); // 리스트 변경 후 UI 갱신
    }

}
