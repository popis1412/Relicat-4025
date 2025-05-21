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

    // ���� ���ΰ�ħ
    public void FreshSlot()
    {
        int i = 0;
        for (; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];

            // UI �ؽ�Ʈ ������Ʈ
            Transform parent = slots[i].transform.parent;
            TextMeshProUGUI childtext = parent.GetChild(1).GetComponent<TextMeshProUGUI>();
            childtext.text = items[i].count.ToString();
        }

        for (; i < slots.Length; i++)
        {
            slots[i].item = null;

            // �� �����̸� �ؽ�Ʈ ����
            Transform parent = slots[i].transform.parent;
            TextMeshProUGUI childtext = parent.GetChild(1).GetComponent<TextMeshProUGUI>();
            childtext.text = "";
        }
    }

    // addEA: ����, _item: Item Object
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
                    }
                    
                }
                FreshSlot();
                return;
            }
        }

        // ���ο� ������ �߰� (���� ������ ���� ���)
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
            Debug.Log("������ ���� �� �ֽ��ϴ�.");
        }
    }

    // �������� ������ �ʱ�ȭ
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

    // Ư�� ������ �Ǹ�
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

                break; // �������� �����ϴٰ� ����
            }
        }

        FreshSlot(); // ����Ʈ ���� �� UI ����
    }

    // Ư�� ������ ���� �Ǹ�
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
                break; // �������� �����ϴٰ� ����
            }
        }

        FreshSlot(); // ����Ʈ ���� �� UI ����
    }
}
