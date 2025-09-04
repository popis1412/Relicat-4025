<<<<<<< Updated upstream
using System.Collections.Generic;
=======
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
using UnityEngine;

public class Tool : MonoBehaviour
{
    public List<GameObject> torchObj = new List<GameObject>();
    [SerializeField] private GameObject bombPrefab;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
    public GameObject torchPrefab;
    [SerializeField] private Vector2 itemSpawnParent;

    private ItemInstance currentItemInstance; // 현재 장착된 아이템
    private SlotInfo currentItemSlot; // 해당 아이템이 들어있는 슬롯

    private static Tool _instance;
    public static Tool Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<Tool>();

                if(_instance == null)
                {
                    GameObject obj = new GameObject();
                    GameObject player = GameObject.FindGameObjectWithTag("Player");

                    obj.transform.SetParent(player.transform);

                    obj.name = typeof(Tool).Name;
                    obj.AddComponent<Transform>();
                    obj.AddComponent<SpriteRenderer>();
                    _instance = obj.AddComponent<Tool>();
                }
            }

            return _instance;
        }
    }
=======
    [SerializeField] private GameObject torchPrefab;

=======
    [SerializeField] private GameObject torchPrefab;

>>>>>>> Stashed changes
    private Tool _instance;
    public static Tool Instance;
>>>>>>> Stashed changes

    public WeaponType currentWeaponType;

    private Component currentWeaponComponent;
    [SerializeField] private SpriteRenderer sprite;

    private void Awake()
    {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        sprite = GetComponent<SpriteRenderer>();

        if(_instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // 무기 장착 + 표시
    public void Equip(SlotInfo newSlot)
    {
        RemoveCurrentWeaponComponent();

        // 무기 인스턴스 우선 → 없으면 아이템 인스턴스
        var instanceW = newSlot._instanceW;
        var instanceI = newSlot._instanceI;

        if(instanceW != null)
        {
            SetWeaponSprite(instanceW);
            ReplaceToolComponent(instanceW); // T = WeaponInstance
        }
        else if(instanceI != null)
        {
            SetWeaponSprite(instanceI);
            ReplaceToolComponent(instanceI); // T = ItemInstance
        }
    }

    // 무기 사용
<<<<<<< Updated upstream
<<<<<<< Updated upstream
    public void UseWeapon(Vector2 mousePos, Player player)
=======
    public void UseWeapon(Vector2 mousePos, Player player, WeaponType type)
>>>>>>> Stashed changes
=======
    public void UseWeapon(Vector2 mousePos, Player player, WeaponType type)
>>>>>>> Stashed changes
    {
        if(currentWeaponComponent == null)
            return;

<<<<<<< Updated upstream
<<<<<<< Updated upstream
        PlayerController _player = player.GetComponent<PlayerController>();
        bool isSanded = _player.ISInSand;   // 모래 갇힘 상태

        if(currentWeaponComponent is Pickaxe pickaxe)
        {
            pickaxe.Digging(mousePos, player, isSanded);
        }
        else if(currentWeaponComponent is Drill drill)
        {
            drill.Digging(mousePos, player, isSanded);
        }
    }

    // 아이템 사용
    public void UseItem(bool isGrounded)
    {
        itemSpawnParent = GameObject.FindGameObjectWithTag("Player").transform.position;

        if(currentItemInstance == null || currentItemInstance._item.count <= 0)
        {
            Debug.LogWarning("사용 가능한 아이템이 없습니다.");
=======
        switch(type)
        {
            case WeaponType.Pickaxe:
                if(currentWeaponComponent is Pickaxe pickaxe)
                    pickaxe.Digging(mousePos, player);
                break;
            case WeaponType.Drill:
                if(currentWeaponComponent is Drill drill)
                {
                    // 처음 한 번만 바인딩하도록 체크
                    if(drill.OnDecreaseEnergy == null)
                        drill.OnDecreaseEnergy = SlotManager.Instance.BindDrillEnergy;
                    
                    drill.Digging(mousePos, player);
                }
                break;
            default:
                break;
        }
    }

    public void UseItem(SlotInfo slot, Transform spawnTransform)
    {
        if(slot == null || slot._instanceI == null)
>>>>>>> Stashed changes
=======
        switch(type)
        {
            case WeaponType.Pickaxe:
                if(currentWeaponComponent is Pickaxe pickaxe)
                    pickaxe.Digging(mousePos, player);
                break;
            case WeaponType.Drill:
                if(currentWeaponComponent is Drill drill)
                {
                    // 처음 한 번만 바인딩하도록 체크
                    if(drill.OnDecreaseEnergy == null)
                        drill.OnDecreaseEnergy = SlotManager.Instance.BindDrillEnergy;
                    
                    drill.Digging(mousePos, player);
                }
                break;
            default:
                break;
        }
    }

    public void UseItem(SlotInfo slot, Transform spawnTransform)
    {
        if(slot == null || slot._instanceI == null)
>>>>>>> Stashed changes
            return;

        var itemInstance = slot._instanceI;

        // 아이템 수 감소
        itemInstance._count--;
        // UI 텍스트 갱신
        SlotManager.Instance.UpdateSlotUI(slot, slot._instanceI._item);

        // 개수 0 이하일 경우 슬롯 비움
        if(itemInstance._count <= 0)
        {
            slot.GetComponentInChildren<SlotInteraction>()?.Clear();
        }

        // 프리팹 찾기
        GameObject prefab = null;

        switch(itemInstance._item.type)
        {
            case ItemType.Bomb:
                prefab = bombPrefab;
                
                break;
            case ItemType.Torch:
                if(!isGrounded)
                {
                    print("횃불은 공중에서 설치할 수 없습니다.");
                    return;
                }
                prefab = torchPrefab;
                
                // 횟불 설치 시 플레이어의 피벗이 (0,0) 위치에 설치 -> 나중에 블럭의 중앙 위치에 설치하게 할 것임.
                SpriteRenderer sr = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
                float playerSize = sr.size.y;
                float torchSize = torchPrefab.GetComponent<SpriteRenderer>().size.y;
                itemSpawnParent = new Vector3(Mathf.Round(transform.position.x - 0.5f) + 0.5f, transform.position.y - (playerSize - torchSize));
                break;
            case ItemType.Teleport:
<<<<<<< Updated upstream
<<<<<<< Updated upstream
                if(!isGrounded)
                {
                    print("텔레포트는 공중에서 사용할 수 없습니다");
                    return;
                }
                Teleport teleport = new Teleport();
                teleport.Spawn(transform.parent.GetComponent<PlayerController>());
                break;

=======
                prefab = null;
                break;
>>>>>>> Stashed changes
=======
                prefab = null;
                break;
>>>>>>> Stashed changes
            default:
                Debug.LogWarning("지원되지 않는 아이템 타입입니다.");
                return;
        }

        // 프리팹 클론 생성
        if(prefab != null && spawnTransform != null)
        {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            GameObject clone = Instantiate(prefab, itemSpawnParent, Quaternion.identity);
            clone.transform.position = transform.position; // 플레이어 위치에서 생성 (원하면 조정 가능)
            clone.transform.SetParent(null); // 부모 움직임의 영향 주지 않기

            if(prefab == torchPrefab)
                torchObj.Add(clone);
        }

        // 개수 감소
        currentItemInstance._item.count--;
        // UI 갱신
        SlotManager.Instance.UpdateSlotUI(currentItemSlot, currentItemInstance._item);
=======
            Instantiate(prefab, spawnTransform.position, Quaternion.identity);
        }
        else if(currentWeaponComponent is Teleport teleport)
        {
            teleport.Spawn();
        }
=======
            Instantiate(prefab, spawnTransform.position, Quaternion.identity);
        }
        else if(currentWeaponComponent is Teleport teleport)
        {
            teleport.Spawn();
        }

    }
>>>>>>> Stashed changes

>>>>>>> Stashed changes
    }

    // 장착 해제
    public void Unequip()
    {
        sprite.sprite = null;
        RemoveCurrentWeaponComponent();
    }

    // 무기 스프라이트 변경
    public void SetWeaponSprite<T>(T instance)
    {
        if(instance is WeaponInstance weapon)
        {
            sprite.sprite = weapon == null ? null : weapon.GetSprite();
        }
        else if(instance is ItemInstance item)
        {
            sprite.sprite = item == null ? null: item.GetSprite();
        }
    }

    // 타입에 맞는 무기 데이터 넣기
    public void ReplaceToolComponent<T>(T instance)
    {
        

        if(instance == null) return;

        Component toolComponent = null;

        // 무기 타입 처리
        if(instance is WeaponInstance weapon)
        {
            switch(weapon._template.type)
            {
                case WeaponType.Pickaxe:
                    toolComponent = gameObject.AddComponent<Pickaxe>();
                    ((Pickaxe)toolComponent).Setup(weapon);
                    break;

                case WeaponType.Drill:
                    toolComponent = gameObject.AddComponent<Drill>();
                    ((Drill)toolComponent).Setup(weapon);
                    break;
            }
        }
        else if(instance is ItemInstance item)
        {
            switch(item._item.type)
            {
                case ItemType.Teleport:
                    toolComponent = gameObject.AddComponent<Teleport>();
                    break;
                default:
                    break;

            }
        }
<<<<<<< Updated upstream

        RemoveCurrentWeaponComponent();
=======
>>>>>>> Stashed changes

        // 현재 장착된 무기 컴포넌트로 등록
        currentWeaponComponent = toolComponent;
    }

    // 기존 무기 컴포넌트 제거
    public void RemoveCurrentWeaponComponent()
    {
        if(currentWeaponComponent != null)
        {
            Destroy(currentWeaponComponent);
            currentWeaponComponent = null;
        }
    }
}
