using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SlotData
{
    public string slotName;
    public Image background;
    public Image item;
    public Sprite currentSprite;
    // �帱
    public Slider energyGauge;
}

public class QuitSlot : MonoBehaviour
{
    private static QuitSlot _instance;

    public static QuitSlot Instance
    {
        get
        {
            if(_instance == null)
            {
                return null;
            }
            return _instance;
        }
    }

    [SerializeField] private List<SlotData> slots = new List<SlotData>();
    int count;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }

        // �ڵ� Ž�� �� ����Ʈ�� ä���
        slots.Clear();
        foreach(Transform slot in transform)
        {
            var selectSlot = slot.Find("select_Slot");
            if(selectSlot != null)
            {
                SlotData data = new SlotData();
                data.slotName = slot.name;
                data.background = selectSlot.GetComponent<Image>();
                data.item = selectSlot.Find("Item").GetComponent<Image>();
                data.currentSprite = data.item.sprite;
                slots.Add(data);
            }
        }
    }

    private void Start()
    {
        Init();
    }

    // �Է¿� ���� �̹��� ����
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectSlotBackground(0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectSlotBackground(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            
        }
    }

    // �ʱ�ȭ
    private void Init()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            slots[i].item.color = new Color(0,0,0,0);
        }
    }

    // �� ������ �� ��á���� Ȯ��
    public bool IsFull()
    {
        foreach(var slot in slots)
        {
            if(slot.currentSprite == null)
                return false;
        }
        return true;
    }

    // �����Կ� ������ �߰�
    public bool TryAddItem(Item itemData, Inventory inventory, int count)
    {
        if(!IsFull())
        {
            // 1. ���� ������� �� ���� ã��
            for(int i = 0; i < slots.Count; i++)
            {
                ChangeColorItem(i);

                if(slots[i].currentSprite == null) // �� ���� �߰�
                {
                    slots[i].currentSprite = itemData.itemImage;
                    slots[i].item.sprite = itemData.itemImage;

                    Debug.Log($"������ {i + 1}���� ������ �߰���");
                    return true; // �� ���� �߰� �� ����
                }
            }
        }
        else
        {
            // 2. ������ ���� �� á���� �κ��丮�� �̵�
            Debug.Log("�������� �� á���ϴ�. �κ��丮�� �߰��մϴ�.");
            inventory.AddItem(itemData, count);
            inventory.FreshSlot();
            return false;
        }

        return false;
    }
    
    // ������ �߰� �� �� �⺻ �̹��� �÷�
    private void ChangeColorItem(int index)
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if(i == index)
            {
                // �ش� ������
                slots[i].item.color = new Color(1,1,1,1);
            }
        }
    }

    // �κ��丮 â�� ������ ���� �̹��� ����

    // ���� ������ ���� ���� ǥ��
    public void SelectSlotBackground(int index)
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if(i == index)
            {
                // �ش� ������
                slots[i].background.color = new Color(109f / 255f, 126f / 255f, 253f / 255f, 100f / 255f);
            }
            else
            {
                slots[i].background.color = new Color(0, 0, 0, 0);
            }
        }
    }

    // ����� ������ ����
    public List<string> GetInventoryState()
    {
        List<string> spriteNames = new List<string>();
        foreach(var s in slots)
        {
            spriteNames.Add(s.currentSprite.name);
        }
        return spriteNames;
    }

    // �巡�� �� ���
}
