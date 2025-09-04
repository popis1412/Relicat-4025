using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.VFX;
//using UnityEngine.Windows;
using System.IO;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif


// 싱글톤을 만들 때 참조 필드를 만들지 말고 메소드 아니면 이 Manager에 필요한 데이터들의 필드만 있을 것(참조가 필요하다면 매개변수를 쓸 것)
public class SlotManager : MonoBehaviour
{
    #region Field
    public GameObject energy;

    private static SlotManager _instance;
    public static SlotManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<SlotManager>();

                if(_instance == null)
                {
                    return null;
                }
            }

            return _instance;
        }
    }

    [SerializeField] private Player player;

    // 액션 입력
    public System.Action<bool> OnInventoryOpen; // 인벤토리 열림
    public Action PickupEnergy; // 드릴 배터리 채우기


    // 인벤토리들
    [SerializeField] public QuitSlotUI quitSlotUI;
    [SerializeField] private InventoryUI inventoyUI;
    [SerializeField] private Inventory inventory;

    public SlotInfo selectedSlot { get; private set; }  // 선택한 슬롯 정보 

    public bool _isOpen;

    // 무기
    public SlotInfo currentWeapon { get; set; } // 현재 장착 무기 정보

    private string savePath => Application.persistentDataPath + "/SaveData.json";

    #endregion Field

    #region Event
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        quitSlotUI = GameObject.FindObjectOfType<QuitSlotUI>();
        quitSlotUI.InitFillData();  // 퀵슬롯 SlotInfo -> 무기/아이템 데이터 채우기
        inventoyUI = GameObject.FindObjectOfType<InventoryUI>();
        inventoyUI.InitFillData(); // 인벤토리 SlotInfo -> 무기/아이템 데이터 채우기
        inventory = GameObject.FindObjectOfType<Inventory>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    
    private void OnEnable()
    {           
        SceneManager.sceneLoaded += OnSceneLoaded;
        PickupEnergy = PrintEvent;
    }

    void PrintEvent()
    {
        print("이벤트 등록");
    }

    private void Start()
    {
        /// <TOOD> 
        /// Player의 Start가 코루틴이고 한 프레임을 뒤에 실행하는 이상 뒤 늦게 하기 위해서는 Invoke()밖에 없음. 
        /// 해결하기 위해서는 GameManager가 저장/로드를 관리를 하고 
        /// OnSceneLoad 이벤트에서 Load()를 하는 방법을 추천함.</TOOD>        
    }

    private void Update()
    {
        for(int i = 0; i < quitSlotUI.quickSlots.Count; i++)
        {
            if(UnityEngine.Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                // 인벤토리가 열렸을 경우에만 클릭 가능
                if(_isOpen && selectedSlot != null)
                {
                    // 데이터 교환 및 슬롯 선택
                    RouteInputToTarget(i);
                }
                // 인벤토리가 열리지 않은 상태에서는 무기 세팅
                else if(!_isOpen && selectedSlot == null)
                {
                    // 무기 선택
                    EquipWeapon(quitSlotUI.quickSlots[i]);
                }
            }
        }
    }
   
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region LoadScene

    // 퀵슬롯 비활성화
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 0 || scene.buildIndex == 1 || scene.name == "Ending")
        {
            var quickslotUIObj = GetComponentInChildren<QuitSlotUI>(true);

            if(quickslotUIObj != null)
            {
                quickslotUIObj.gameObject.SetActive(false);
            }
        }
        else
        {
            var quickslotUIObj = GetComponentInChildren<QuitSlotUI>(true);

            if(quickslotUIObj != null)
            {
                quickslotUIObj.gameObject.SetActive(true);
                //SaveSystem.Instance.Load();
            }
        }
    }

    #endregion LoadScene

    #endregion Event

    #region Func

    public void BindPlayer(Player p)
    {
        player = p;
    }

    // 레벨에 따른 초기 세팅들
    public void Init()
    {
        if(SceneManager.GetActiveScene().buildIndex == 2)   // Tutorial
        {
            currentWeapon = null;
            FillSlot(null, player.Weapons[0], 1);   // 곡괭이
            BindInitWeapon();   // 초기 곡괭이
            InitFillSlot();
        }
        else if(SceneManager.GetActiveScene().buildIndex == 3 && !File.Exists(savePath))  // Main
        {
            quitSlotUI.ResetQuickSlot();    // 퀵슬롯 아이템 초기화
            currentWeapon = null;   // 현재 무기 상태도 초기화
            FillSlot(null, player.Weapons[0], 1);   // 곡괭이
            BindInitWeapon();   // 초기 곡괭이
            InitFillSlot();
        }
    }

    // 인벤토리 열었는 지 여부
    public void HandleInventoryOpen(bool isOpen)
    {
        _isOpen = isOpen;
    }

    // 곡괭이 무기 장착
    public void BindInitWeapon()
    {
        // 무기 세팅
        foreach(var slot in quitSlotUI.quickSlots)
        {
            if(slot._instanceW != null && slot._instanceW._template.type == WeaponType.Pickaxe)
            {
                EquipWeapon(slot);
                break;
            }
        }
    }

    // 무기 장착
    public void EquipWeapon(SlotInfo newSlot)
    {
        //이미 선택한 무기라면 기존의 것 표시
        if (newSlot == currentWeapon)
            return;

        //기존 무기 하이라이트 해제
        if (currentWeapon != null)
            SetWeaponHighlight(currentWeapon, false);

        // 무기라면 무기 장착 + 하이라이트
        if(newSlot._instanceW != null || newSlot._instanceI != null)
        {
            Tool.Instance.Equip(newSlot);
            SetWeaponHighlight(newSlot, true);
            currentWeapon = newSlot;
        }
        else
        {
            // 무기 없으면 장착 안 함
            currentWeapon = null;
            Debug.Log("장착할 무기나 아이템이 없습니다.");
        }
    }

    // [숫자 입력 교환] 슬롯끼리의 데이터 교환
    private void RouteInputToTarget(int quickIndex)
    {
        // 교환할 슬롯 데이터 -> 숫자 입력값
        SlotInfo targetSlot = quitSlotUI.quickSlots[quickIndex];

        if(selectedSlot._typeS == SlotType.QuickSlot)
        {
            SwapQuickSlot(selectedSlot, targetSlot);

            // 무기였다면 현재 장착 무기 변경
            if(selectedSlot._instanceW != null || targetSlot._instanceW != null)
            {
                // 무기 교환이 일어났다면, 새 슬롯을 기준으로 무기 장착 로직 재실행
                if(targetSlot._instanceW != null)
                {
                    EquipWeapon(targetSlot);
                }
                else if(selectedSlot._instanceW != null)
                {
                    EquipWeapon(selectedSlot);
                }
            }
        }
        else if(selectedSlot._typeS == SlotType.Inventory)
        {
            SwapInventroySlot(selectedSlot.GetComponentInChildren<Slot>().item, quickIndex);
        }

        selectedSlot = null;
    }

    // 데이터 교환
    private void SwapQuickSlot(SlotInfo from, SlotInfo to)
    {
        SetSlotHighlight(selectedSlot, false);  // 선택한 슬롯 배경 제거

        (from._instanceW, to._instanceW) = (to._instanceW, from._instanceW);
        (from._instanceI, to._instanceI) = (to._instanceI, from._instanceI);

        from.GetComponentInChildren<SlotInteraction>()?.SetSlotInfo(from);
        to.GetComponentInChildren<SlotInteraction>()?.SetSlotInfo(to);

        UpdateText(from);
        UpdateText(to);
    }

    /*// 인벤토리 -> 퀵슬롯
    private void SwapInventroySlot(Item form, SlotInfo to)
    {
        int limit = form.stackLimit;
        int availableToAdd = Mathf.Min(form.count, limit);

        var toInstance = to._instanceI;

        if(toInstance == null)
        {
            var newInstance = new ItemInstance(form);
            newInstance._count = availableToAdd;
            to._instanceI = newInstance;
            form.count -= availableToAdd;

            // UI 반영
            UpdateSlotUI(to, form);
            return;
        }

        // 2. 같은 아이템일 경우 → 병합
        if(toInstance._item.itemName == form.itemName)
        {
            int current = toInstance._count;

            // 이미 꽉 참
            if(current >= limit)
            {
                Debug.Log($"[슬롯] 이미 {limit}개로 꽉 찼습니다.");
                return;
            }

            int space = limit - current;
            int toAdd = Mathf.Min(space, form.count);

            toInstance._count += toAdd;
            form.count -= toAdd;

            UpdateSlotUI(to, form);
            return;
        }

        // 3. 다른 아이템이 이미 있음
        Debug.LogWarning($"[슬롯] 이미 다른 아이템이 존재합니다. '{toInstance._item.itemName}' 슬롯에 '{form.itemName}'를 넣을 수 없습니다.");
    }*/

    // 인벤토리 -> 퀵슬롯
    private void SwapInventroySlot(Item selectedItem, int targetIndex)
    {
        if(selectedItem.type != ItemType.Null)  // 지정한 아이템을 제외한 나머지는 스왑 못함. - 유물, 업그레이드, 드릴 등
        {
            SetSlotHighlight(selectedSlot, false);  // 선택한 슬롯 배경 제거

            SlotInfo targetSlot = quitSlotUI.quickSlots[targetIndex];   // 변경될 퀵슬롯
            string selecteditemName = selectedItem.itemName;            // 인벤토리 아이템 이름

            int maxCount = selectedItem.stackLimit;                     // 최대 개수
            int toMoveCount = Mathf.Min(maxCount, selectedItem.count);       // 인벤토리의 갯수 이동

            ItemInstance targetInstance = targetSlot._instanceI;

            // 퀵슬롯의 같은 이름의 아이템 인덱스 찾기
            List<SlotInfo> sameItemSlots = quitSlotUI.quickSlots
                .Where(s => s._instanceI != null &&
                            s._instanceI._item.itemName == selecteditemName)
                .ToList();


            // 같은 이름의 아이템이 있는 슬롯이면 채우지 못하지만, 개수가 최대치 이하이다.라고 한다면 최대치가 될 남은 개수를 넣고 인벤토리에서 뺀다.
            // 1. 기존에 있던 슬롯들 중 부족한 슬롯 채우기
            for(int i = 0; i < sameItemSlots.Count; i++)
            {
                var slot = sameItemSlots[i];
                var inst = slot._instanceI;

                if(inst._count < maxCount)
                {
                    int space = maxCount - inst._count;
                    int toAdd = Mathf.Min(space, selectedItem.count);

                    inst._count += toAdd;   // 퀵슬롯
                    selectedItem.count -= toAdd; // 인벤토리

                    print($"[기존 추가] {selecteditemName}의 수량을 {toAdd}만큼  기존 슬롯({slot.name})에 추가했습니다. ");
                }
                else if(inst._count >= maxCount)
                {
                    print($"[최대] '{selecteditemName}'은 이미 슬롯에 있고, 최대치입니다.");
                    if(i > sameItemSlots.Count)
                    {
                        UpdateSlotUI(slot, selectedItem);
                        return;
                    }
                    continue;
                }

                UpdateSlotUI(slot, selectedItem);
                return;
            }

            // 같은 이름의 슬롯이 아닌데, 이미 다른 데이터가 있어. 그러면 선택한 데이터의 개수와 데이터를 넣고, 그 이미 있는 데이터에서 같은 아이템을 인벤토리에서 찾아서
            // count를 추가를 해.
            if(targetInstance != null)
            {
                inventory.AddItem(targetInstance._item, targetInstance._count);

                var newInstance = new ItemInstance(selectedItem, toMoveCount);
                targetSlot._instanceI = newInstance;

                selectedItem.count -= toMoveCount;  // 인벤토리 아이템 개수 제거

                print($"[교체] 슬롯 {targetIndex}의 아이템을 교체하고 '{selecteditemName}'을 넣었습니다.");
            }
            // Case 3. 같은 이름의 슬롯은 아닌데, 데이터가 없어. null이야. 그러면 선택한 데이터의 개수와 이미지들을 넣고 하면 끝.
            else if(targetInstance == null && targetSlot._instanceW == null)
            {
                var newInstance = new ItemInstance(selectedItem, toMoveCount);
                targetSlot._instanceI = newInstance;

                selectedItem.count -= toMoveCount;  // 인벤토리 아이템 개수 제거

                Debug.Log($"[새로운 추가] '{selecteditemName}'을 퀵슬롯 {targetIndex}에 추가했습니다.");
            }

            UpdateSlotUI(targetSlot, selectedItem);

        }
    }

    // 선택된 배경 조건들(선택)
    public void SetSelectedSlot(SlotInfo slot)
    {
        // 1. 초기에 슬롯이 아무것도 선택되지 않은 경우
        if(selectedSlot == null)
        {
            selectedSlot = slot;
            SetSlotHighlight(slot, true);
            return;
        }

        // 2. 같은 슬롯을 선택한 경우
        if(selectedSlot._index == slot._index)
        {
            SetSlotHighlight(slot, false);

            if(slot == currentWeapon)
            {
                SetWeaponHighlight(slot, true); // 무기 하이라이트 다시 켜기
            }

            selectedSlot = null;
            return;
        }

        // 3. 이미 선택된 상태에서 다른 슬롯을 클릭한 경우
        SetSlotHighlight(selectedSlot, false);
        selectedSlot = slot;
        SetSlotHighlight(slot, true);

        //Debug.Log($"슬롯 {slot._index} 선택됨: {slot._instance?._template.name}");
    }  

    // 해당 슬롯의 무기 제거
    public void RemoveInstance(SlotInfo slot)
    {
        // 무기나 아이템 인스턴스 제거
        if(slot._instanceW != null || slot._instanceI != null)
        {
            // 인스턴스 제거
            slot._instanceW = null;
            slot._instanceI = null;

            // UI 클리어
            SetSlotHighlight(slot, false);
            slot.GetComponentInChildren<SlotInteraction>().Clear();

            UpdateText(slot);

            Debug.Log($"슬롯 {slot._index}의 아이템/무기 제거됨.");
        }
    }

    // 아이템 배경 색깔 변경
    private void SetSlotHighlight(SlotInfo slot, bool selected)
    {
        var bg = slot.GetComponentInChildren<SlotBackground>();
        bg.SetSelected(selected);
    }

   // 무기 배경 색깔 변경
   public void SetWeaponHighlight(SlotInfo slot, bool hasWeapon)
   {
        var bg = slot.GetComponentInChildren<SlotBackground>(true);
        bg.SetEquipped(hasWeapon);
   }

    /* 슬롯들 채우기(아이템)[아이템 최대 개수, 인벤토리에 다른 아이템을 집어 넣을 때 사용]
    public void FillSlot(Item template, int addEA)
    {
        int remaining = addEA;

        for(int i = 0; i < addEA; i++)
        {
            if(remaining <= 0)
                break;

            // 1. 퀵슬롯
            SlotInfo slot = quitSlotUI.FindEmptySlot(false, template.itemName);

            // 2. 인벤토리
            if(slot == null)
            {
                inventory.AddItem(template, remaining);
                Debug.Log($"[퀵슬롯] 꽉 참. 인벤토리로 {addEA}개 이동.");
                return;
            }           
            int added = TryFillSlot(slot, template, remaining);
            remaining -= added;

            UpdateText(slot);
        }
        inventory.ItemLog(template, addEA);
    }*/

    // 초기 슬롯들 아이템 채우기(아이템)
    public void InitFillSlot()
    {
        // 인벤토리에 있는 아이템들 가져오기
        foreach(Slot item in inventory.Slots)
        {
            if(item.item == null)
                return;

            int _count = item.item.count;
            string _name = item.item.name;

            // 1. 퀵슬롯
            SlotInfo slot = FindSlot(item.item.type);

            ItemInstance instance = new ItemInstance(item.item);
            slot._instanceI = instance;
            slot._instanceW = null;

            slot._instanceI._count = item.item.count;

            // 나중에 아이템에 대한 제한 개수가 있다면 쓸 예정
            /*if(item.item.count > item.item.stackLimit) // 아이템의 개수가 최대치보다 많다면
            {
                slot._instanceI._count = item.item.stackLimit;  // 최대치로 계수 저장
                item.item.count -= slot._instanceI._count;
            }
            else if(item.item.count <= item.item.stackLimit) // 아이템이 최대치보다 작다면
            {
                slot._instanceI._count = item.item.count;
                item.item.count -= slot._instanceI._count;  // 현재 개수를 넣기
            }*/

            UpdateSlotUI(slot, item.item);
        }
    }

    public void LoadQuickSlots(SaveData data)
    {
        // 1. 기존 슬롯 초기화
        foreach(var slot in quitSlotUI.quickSlots)
        {
            // 슬롯 내 상호작용 요소 클리어
            slot.GetComponentInChildren<SlotInteraction>()?.Clear();

            // 슬롯 UI 텍스트 업데이트 (아이템 개수 등)
            UpdateText(slot);
        }

        // 2. 저장된 슬롯 데이터 기준으로 슬롯 채우기
        foreach(var saved in data.quickSlotInfoData.slots)
        {
            var slot = quitSlotUI.quickSlots.FirstOrDefault(s => s._index == saved.index);
            if(slot == null) continue;

            slot._index = saved.index;
            slot._typeS = saved.type;

            // 유효한 무기만 복원
            slot._instanceW = saved.instanceW?._template != null ? saved.instanceW : null;

            // 유효한 아이템만 복원
            slot._instanceI = saved.instanceI?._item != null ? saved.instanceI : null;

            // UI 업데이트
            UpdateSlotUI(slot, slot._instanceI?._item);
        }


        // 3. 현재 장착 무기 복원
        if(data.quickSlotInfoData.currentWeapon != null)
        {
            int weaponIndex = data.quickSlotInfoData.currentWeapon.index;

            var weaponSlot = quitSlotUI.quickSlots.FirstOrDefault(s => s._index == weaponIndex);
            if(weaponSlot != null && weaponSlot._instanceW != null)
            {
                EquipWeapon(weaponSlot);
            }
        }
    }

    // 슬롯들 채우기(아이템)
    public void FillSlot(Item template, int addEA)
    {
        int remaining = addEA;

        
            SlotInfo slot = FindSlot(template.type);

            // 2. 인벤토리
            if(slot == null)
            {
                Debug.Log($"[퀵슬롯] 꽉 참");
                return;
            }

            int added = TryFillSlot(slot, template, remaining);
            
        
    }

    // 슬롯 채우기(무기)
    public void FillSlot(Item item, WeaponTemplate template, int addEA)
    {
        // 1. 퀵슬롯
        SlotInfo slot = FindSlot(template.type);

        // 2. 인벤토리
        if(slot == null)
        {
            Debug.Log($"[퀵슬롯] 꽉 참");
            return;
        }

        WeaponInstance instance = new WeaponInstance(template, slot._index);

        slot._instanceW = instance;
        slot._instanceI = null;

        UpdateSlotUI(slot, item);

        //if(item != null)
        //{
        //    inventory.ItemLog(item, addEA);
        //}
    }

    // 퀵슬롯에 데이터들 채우기
    private int TryFillSlot(SlotInfo slot, Item template, int available)
    {
        ItemInstance instance = GetOrCreateInstance(slot, template);

        slot._instanceI = instance;
        slot._instanceW = null;

        int toAdd = available;

        if(slot._typeS == SlotType.QuickSlot)
        {
            //int space = instance._item.stackLimit - instance._count;
            //toAdd = Mathf.Min(space, available);

            instance._count += toAdd;
        }

        UpdateSlotUI(slot, template);

        return toAdd;
    }

    // 아이템 인스턴스 가져오기(최대 개수가 되기 전까지는 기존 것 반환 / 새로운 인스턴스 반환)
    private ItemInstance GetOrCreateInstance(SlotInfo slot, Item template)
    {
        if(slot._instanceI != null)
            if(slot._instanceI._item != null)
                return slot._instanceI;

        var instance = new ItemInstance(template);
        return instance;
    }

    // 슬롯 이미지들과 데이터들 업데이트
    public void UpdateSlotUI(SlotInfo slot, Item item)
    {
        var interaction = slot.GetComponentInChildren<SlotInteraction>();

        if(interaction == null)
            return;
        
        if(slot._instanceI == null && slot._instanceW == null)
        {
            interaction.Apply(item);    // 새 아아템 장착
        }
        else if(slot._instanceI == null && slot._instanceW != null)
        {
            interaction.Apply(slot._instanceW); // 무기 장착
        }
        else if(slot._instanceW == null && slot._instanceI != null)
        {
            if(slot._instanceI._item.count >= 0)    
            {
                interaction.Apply(item);    // 아이템 장착
            }

            //interaction.Clear(); // 나중에 카운트가 따로 있다면 사용 -> 인벤토리 0개 시 초기화
        }

        SetSlotHighlight(slot, false);
        UpdateText(slot);
    }

    // 텍스트 업데이트
    public void UpdateText(SlotInfo slot)
    {
        var text = slot.GetComponentInChildren<TextMeshProUGUI>(true);  // 비활성화 상태에서도 찾기
        EnergyBar _bar = slot.GetComponentInChildren<EnergyBar>(true);
        if(_bar != null)
        {
            Destroy(_bar.gameObject);
        }

        if(text != null && slot._instanceI == null && slot._instanceW == null)    // 아무런 데이터가 없을 때
        {
            text.enabled = false;
            text.text = "0";
        }
        else if(text != null && slot._instanceI != null)    // 아이템
        {
            text.enabled = true;
            //text.text = slot._instanceI._count.ToString(); // 나중에 인벤토리와 따로 연관이 된다면 사용
            text.text = slot._instanceI._item.count.ToString();
        }
        else if(text != null && slot._instanceW != null) // 무기
        {
            text.enabled = false;

            if(slot._instanceW._template.type == WeaponType.Drill)
            {
                GameObject bar = Instantiate(energy, text.transform.parent);
                bar.GetComponent<EnergyBar>().SetValue(100);
            }
        }

        // 인벤토리 텍스트 초기화
        inventory.FreshSlot();
    }

    // 무기 업그레이드 이미지 가져오기
    public void UpgradeWeapon(SlotInfo slot)
    {
        if(slot._instanceI == null && slot._instanceW != null)
        {
            switch(slot._instanceW._template.type)
            {
                case WeaponType.Pickaxe:
                    slot._instanceW._level += 1;
                    UpdateSlotUI(slot, null);
                    Tool.Instance.Equip(slot);
                    break;
                default:
                    break;
            }
        }
    }

    // 드릴 에너지 감소 및 충전
    public void BindDrillEnergy(Drill drill, float energy)
    {
        // 1. 슬롯에 드릴이 있는지 확인
        SlotInfo slot = quitSlotUI.FindDrill(drill);

        if(slot == null)
        {
            print("드릴이 없습니다.");
            return;
        }

        EnergyBar bar = slot.GetComponentInChildren<EnergyBar>(true);
        //bar.SetMax(slot._instanceW._energy); // 나중에 강화 중에서 energy 증가를 위해 남겨둠.

        bar.SetValue(energy);
    }


    // 퀵슬롯에 해당 무기 있는지 확인
    public bool EquipWeaponByType(WeaponType weaponType)
    {
        foreach(var slot in quitSlotUI.quickSlots)
        {
            if(slot._instanceW != null && slot._instanceI == null)
            {
<<<<<<< Updated upstream
                if(slot._instanceW._template.type == weaponType)
                {
                    EquipWeapon(slot);
                    print($"타입: {slot._instanceW._template.type}, 적용된 슬롯: {slot}");
                    return;
                }
=======
                EquipWeapon(slot);
                return true;
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
            }
        }

        Debug.LogWarning($"해당 무기 타입({weaponType})을 가진 슬롯이 없습니다.");
        return false;
<<<<<<< Updated upstream
    }

    public bool IsEquipWeapon(WeaponType weaponType)
    {
        foreach(var slot in quitSlotUI.quickSlots)
        {
            if(slot._instanceW != null && slot._instanceI == null)
            {
                if(slot._instanceW._template.type == weaponType)
                    return true;
            }
        }
        return false;
=======
>>>>>>> Stashed changes
    }

    // 퀵슬롯에 해당 아이템 있는지 확인 
    public void EquipItemByType(ItemType itemType, Transform spawnTransform)
    {
        foreach(var slot in quitSlotUI.quickSlots)
        {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            if(slot._instanceI != null && slot._instanceW == null)
            {
                if(slot._instanceI._item.type == itemType && slot._instanceI._count > 0)
                {
                    EquipWeapon(slot);
                    return;
                }
=======
=======
>>>>>>> Stashed changes
            if(slot._instanceI != null && slot._instanceI._item.type == itemType)
            {
                EquipWeapon(slot);
                Tool.Instance.UseItem(slot, spawnTransform);
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
            }
        }

        Debug.LogWarning($"해당 아이템 타입({itemType})을 가진 슬롯이 없습니다.");
    }

    // 무슨 아이템 먹었는지 확인 -> 해당 슬롯의 데이터 가져오기
    public void ItemPick(Item item, int amount)
    {
        FillSlot(item, amount);
    }

    // 타입을 이용해 아이템 지정석에 아이템들 넣기
    public SlotInfo FindSlot<T>(T typeEnum) where T : System.Enum
    {
        foreach(var slot in quitSlotUI.quickSlots)
        {
            if(typeEnum is WeaponType weapon && slot._typeW == weapon)  // 무기
                return slot;
            else if(typeEnum is ItemType item && slot._typeI == item)   // 아이템
            {
                return slot;
            }
            else
                continue;
        }

        return null;    // 무기가 이미 있는 경우, 빈 슬롯일 경우
    }

    #endregion Func
}
