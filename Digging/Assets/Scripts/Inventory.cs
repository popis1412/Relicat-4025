using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;
using Unity.VisualScripting;

public class Inventory : MonoBehaviour
{
    public List<Item> items;
    public int money;
    [SerializeField] private TextMeshProUGUI money_text;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;

    [SerializeField] private GameObject getItem_LogPanel;
    [SerializeField] private Image getItem_Image;
    [SerializeField] private TextMeshProUGUI getItem_Name;
    [SerializeField] private TextMeshProUGUI getItem_EA;
    private int dupleLog = 1;


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

    // 슬롯 새로고침
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

        money_text.text = money.ToString();

    }

    // addEA: 갯수, _item: Item Object
    public void AddItem(Item _item, int addEA)
    {

        foreach (Item item in items)
        {
            if (item.itemName == _item.itemName)
            {
                _item.count += addEA;
                _item.ishaveitem = true;
                for(int i = 0; i < items.Count; i++)
                {
                    if(slots[i].item.itemName == _item.itemName)
                    {
                        Transform parent = slots[i].transform.parent;
                        TextMeshProUGUI childtext = parent.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                        Debug.Log(childtext);
                        childtext.text = item.count.ToString();

                        ItemLog(_item, addEA);
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

                    ItemLog(_item, addEA);
                }
            }
        }
        else
        {
            Debug.Log("슬롯이 가득 차 있습니다.");
        }
    }

    // 보유중인 아이템 초기화
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

    // 특정 아이템 판매
    public void SellItem(Item _item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == _item.itemName)
            {
                items[i].count--;
                if (items[i].isMineral == true)
                {
                    money += _item.value;
                }
                else if(items[i].isalreadySell == false)
                {
                    money += _item.value;
                    items[i].isalreadySell = true;
                }
                else
                {
                    money += _item.duplicate_value;
                }
                
                if (items[i].count <= 0)
                {
                    items[i].count = 0;
                    items.RemoveAt(i);
                }
                Debug.Log(money);

                break; // 아이템은 유일하다고 가정
            }
        }

        FreshSlot(); // 리스트 변경 후 UI 갱신
    }

    // 특정 아이템 전부 판매
    public void SellAllItem(Item _item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == _item.itemName)
            {
                int idx = items[i].count;
                for (int j = 0; j < idx; j++)
                {
                    items[i].count--;

                    if (items[i].count <= 0)
                    {
                        items[i].count = 0;
                        items.RemoveAt(i);
                    }
                    money += _item.value;
                }
                
                Debug.Log(money);
                break; // 아이템은 유일하다고 가정
            }
        }

        FreshSlot(); // 리스트 변경 후 UI 갱신
    }

    // 아이템 획득 로그
    private void ItemLog(Item _item, int addEA)
    {
        
        getItem_LogPanel.SetActive(true);
        
        // 같은 아이템 연속 획득 시 
        if(getItem_Name.text == _item.itemName)
        {
            getItem_EA.text = "X " + (addEA+dupleLog).ToString();
            dupleLog++;
        }
        // 다른 아이템 획득 시
        else
        {
            getItem_Image.GetComponent<Image>().sprite = _item.itemImage;
            getItem_Name.text = _item.itemName;
            getItem_EA.text = "X " + addEA.ToString();

            dupleLog = 1;
        }
        
        CancelInvoke("closeItemLog");
        Invoke("closeItemLog", 3f);
    }

    // 일정 시간 뒤 로그 패널 닫기
    private void closeItemLog()
    {
        getItem_LogPanel.SetActive(false);
    }
}
