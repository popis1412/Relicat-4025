using Unity.VisualScripting;
using UnityEngine;
using static Spine.Unity.Examples.MixAndMatchSkinsExample;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Tool : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject torchPrefab;
    [SerializeField] private Transform itemSpawnParent;

    private ItemInstance currentItemInstance; // 현재 장착된 아이템
    private SlotInfo currentItemSlot; // 해당 아이템이 들어있는 슬롯

    private static Tool _instance;
    public static Tool Instance;

    public WeaponType currentWeaponType;

    private Component currentWeaponComponent;
    [SerializeField] SpriteRenderer sprite;

    // 일단 마우스 오른쪽 하면 드릴로 바꿔달라는 소리를 했으니
    // 마우스 오른쪽 클릭을 하면 첫번째의 드릴로 자동 선택이 되게 만들어야지. 게다가 그것의 슬롯 선택된 것도 배경 표시를 해야 되고.
    private void Awake()
    {
        if(_instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject); // 중복 방지
        }

        sprite = GetComponent<SpriteRenderer>();
    }

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
            currentItemInstance = null;
            currentItemSlot = null;
        }
        else if(instanceI != null)
        {
            SetWeaponSprite(instanceI);
            currentItemInstance = instanceI;
            currentItemSlot = newSlot;
        }
        else
        {
            return; // 인스턴스가 아무것도 없음
        }
    }

    public void UseWeapon(Vector2 mousePos, Player player)
    {
        if(currentWeaponComponent == null)
            return;

        if(currentWeaponComponent is Pickaxe pickaxe)
        {
            pickaxe.Digging(mousePos, player);
        }
        else if(currentWeaponComponent is Drill drill)
        {
            // 처음 한 번만 바인딩하도록 체크
            if(drill.OnDecreaseEnergy == null)
                drill.OnDecreaseEnergy = SlotManager.Instance.BindDrillEnergy;

            drill.Digging(mousePos, player);
        }
    }

    public void UseItem()
    {
        itemSpawnParent = GameObject.FindGameObjectWithTag("Player").transform;

        if(currentItemInstance == null || currentItemInstance._count <= 0)
        {
            Debug.LogWarning("사용 가능한 아이템이 없습니다.");
            return;
        }

        GameObject prefab = null;

        switch(currentItemInstance._item.type)
        {
            case ItemType.Bomb:
                prefab = bombPrefab;
                break;
            case ItemType.Torch:
                prefab = torchPrefab;
                break;
            default:
                Debug.LogWarning("지원되지 않는 아이템 타입입니다.");
                return;
        }

        if(prefab != null)
        {
            GameObject clone = Instantiate(prefab, itemSpawnParent);
            clone.transform.position = transform.position; // 플레이어 위치에서 생성 (원하면 조정 가능)
        }

        // 개수 감소
        currentItemInstance._count--;

        // UI 갱신
        SlotManager.Instance.UpdateSlotUI(currentItemSlot, currentItemInstance._item);
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
            sprite.sprite = weapon != null ? weapon.GetSprite() : null;
        }
        else if(instance is ItemInstance item)
        {
            sprite.sprite = item != null ? item.GetSprite() : null;
        }
    }

    // 타입에 맞는 무기 데이터 넣기
    public void ReplaceToolComponent<T>(T instance)
    {
        RemoveCurrentWeaponComponent();

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
