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
    // 드릴
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

        // 자동 탐색 후 리스트에 채우기
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

    // 입력에 따른 이미지 변경
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

    // 초기화
    private void Init()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            slots[i].item.color = new Color(0,0,0,0);
        }
    }

    // 퀵 슬롯이 다 꽉찼는지 확인
    public bool IsFull()
    {
        foreach(var slot in slots)
        {
            if(slot.currentSprite == null)
                return false;
        }
        return true;
    }

    // 퀵슬롯에 아이템 추가
    public bool TryAddItem(Item itemData, Inventory inventory, int count)
    {
        if(!IsFull())
        {
            // 1. 슬롯 순서대로 빈 슬롯 찾기
            for(int i = 0; i < slots.Count; i++)
            {
                ChangeColorItem(i);

                if(slots[i].currentSprite == null) // 빈 슬롯 발견
                {
                    slots[i].currentSprite = itemData.itemImage;
                    slots[i].item.sprite = itemData.itemImage;

                    Debug.Log($"퀵슬롯 {i + 1}번에 아이템 추가됨");
                    return true; // 한 번만 추가 후 종료
                }
            }
        }
        else
        {
            // 2. 슬롯이 전부 꽉 찼으면 인벤토리로 이동
            Debug.Log("퀵슬롯이 꽉 찼습니다. 인벤토리에 추가합니다.");
            inventory.AddItem(itemData, count);
            inventory.FreshSlot();
            return false;
        }

        return false;
    }
    
    // 아이템 추가 될 때 기본 이미지 컬러
    private void ChangeColorItem(int index)
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if(i == index)
            {
                // 해당 아이템
                slots[i].item.color = new Color(1,1,1,1);
            }
        }
    }

    // 인벤토리 창이 열렸을 때의 이미지 변경

    // 현재 선택한 슬롯 색깔 표시
    public void SelectSlotBackground(int index)
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if(i == index)
            {
                // 해당 아이템
                slots[i].background.color = new Color(109f / 255f, 126f / 255f, 253f / 255f, 100f / 255f);
            }
            else
            {
                slots[i].background.color = new Color(0, 0, 0, 0);
            }
        }
    }

    // 저장용 데이터 추출
    public List<string> GetInventoryState()
    {
        List<string> spriteNames = new List<string>();
        foreach(var s in slots)
        {
            spriteNames.Add(s.currentSprite.name);
        }
        return spriteNames;
    }

    // 드래그 앤 드롭
}
