using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif
using Unity.VisualScripting;
using System.Collections.ObjectModel;

public class Inventory : MonoBehaviour
{

    public static Inventory instance;

    public Collection collection;

    public List<Item> items;
    public int money;
    public Item money_item;
    [SerializeField] private TextMeshProUGUI money_text;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;

    [SerializeField] private GameObject getItem_LogPanel;
    [SerializeField] private Image getItem_Image;
    [SerializeField] private TextMeshProUGUI getItem_Name;
    [SerializeField] private TextMeshProUGUI getItem_EA;
    private int dupleLog = 0;
    private int dupleMoneyLog = 0;

    public GameObject badgePanel;
    public GameObject moneyPanel;
    public GameObject healthPanel;

    [SerializeField] private GameObject logMessage_Panel;
    [SerializeField] private TextMeshProUGUI logText;

#if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
        
    }
#endif

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(this.gameObject); // �ߺ� ����
        }

        FreshSlot();
    }

    public static Inventory Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
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

        money_text.text = money_item.count.ToString() + " ��";

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

                        ItemLog(_item, addEA);
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
            _item.ishaveitem = true;
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
                if (items[i].count <= 0)
                {
                    items[i].count = 0;
                    items.RemoveAt(i);
                }
                else
                {
                    items[i].count--;
                    if (items[i].isMineral == true)
                    {
                        money_item.count += _item.value;
                        ItemLog(money_item, _item.value);
                    }
                    else if (items[i].isalreadySell == false)
                    {
                        money_item.count += _item.value;
                        items[i].isalreadySell = true;
                        ItemLog(money_item, _item.value);
                    }
                    else
                    {
                        money_item.count += _item.duplicate_value;
                        ItemLog(money_item, _item.duplicate_value);
                    }
                    if (items[i].count <= 0)
                    {
                        items[i].count = 0;
                        items.RemoveAt(i);
                    }
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[32]);
                }
                   
                
                Debug.Log(money_item.count);


                
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
                    if (items[i].count <= 0)
                    {
                        items[i].count = 0;
                        items.RemoveAt(i);
                    }
                    else
                    {
                        items[i].count--;
                        if (items[i].isMineral == true)
                        {
                            money_item.count += _item.value;
                            ItemLog(money_item, _item.value);
                        }
                        else if (items[i].isalreadySell == false)
                        {
                            money_item.count += _item.value;
                            items[i].isalreadySell = true;
                            ItemLog(money_item, _item.value);
                        }
                        else
                        {
                            money_item.count += _item.duplicate_value;
                            ItemLog(money_item, _item.duplicate_value);
                        }
                        if (items[i].count <= 0)
                        {
                            items[i].count = 0;
                            items.RemoveAt(i);
                        }
                    }
                }
                SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[32]);
                Debug.Log(money_item.count);
                break; // �������� �����ϴٰ� ����
            }
        }
        

        FreshSlot(); // ����Ʈ ���� �� UI ����
    }

    // ������ ȹ�� �α�
    public void ItemLog(Item _item, int addEA)
    {
        

        // ���� ������ ���� ȹ�� �� 
        if(getItem_Name.text == _item.itemName)
        {
            if (getItem_LogPanel.activeSelf == false)
            {
                
                getItem_EA.text = "X " + addEA.ToString();
            }
            else if (_item == money_item)
            {
                
                getItem_EA.text = "X " + (dupleMoneyLog + addEA).ToString();
                dupleMoneyLog += addEA;
            }
            else
            {
                dupleLog++;
                getItem_EA.text = "X " + (addEA + dupleLog).ToString();
            }
            
        }
        // �ٸ� ������ ȹ�� ��
        else
        {
            getItem_Image.GetComponent<Image>().sprite = _item.itemImage;
            if (_item.isRelic == true)
            {
                for(int i = 0; i < collection.player.items.Count; i++)
                {
                    if (collection.player.items[i].itemName == _item.itemName)
                    {
                        if(collection.li_isRelicOnTable[i] == false)
                        {
                            getItem_Name.text = "???";
                            LogMessage("???" + "��(��) ȹ���Ͽ����ϴ�." + "\n�ڹ����� ����Ͽ� ��ǥ�� �޼��ϼ���!");
                        }
                        else
                        {
                            getItem_Name.text = _item.itemName;
                            LogMessage(_item.itemName + "��(��) ȹ���Ͽ����ϴ�.");
                        }
                    }
                }
                
            }
            else
            {
                getItem_Name.text = _item.itemName;
            }

            if(_item == money_item)
            {
                dupleMoneyLog += addEA;
            }
            else
            {
                dupleMoneyLog = 0;
            }
            getItem_EA.text = "X " + addEA.ToString();

            dupleLog = 0;
        }

        getItem_LogPanel.SetActive(true);

        CancelInvoke("closeItemLog");
        Invoke("closeItemLog", 3f);
    }

    // ���� �ð� �� �α� �г� �ݱ�
    private void closeItemLog()
    {
        getItem_LogPanel.SetActive(false);
        getItem_Name.text = "";
        getItem_EA.text = "";
        dupleLog = 0;
        dupleMoneyLog = 0;
    }

    public void LogMessage(string log)
    {
        logText.text = log;

        logMessage_Panel.SetActive(true);

        CancelInvoke("closeLogMessage");
        Invoke("closeLogMessage", 3f);
    }
    private void closeLogMessage()
    {
        logMessage_Panel.SetActive(false);
        logText.text = "";
    }
}
