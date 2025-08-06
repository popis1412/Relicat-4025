using Unity.VisualScripting;
using UnityEngine;
using static Spine.Unity.Examples.MixAndMatchSkinsExample;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Tool : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject torchPrefab;
    [SerializeField] private Transform itemSpawnParent;

    private ItemInstance currentItemInstance; // ���� ������ ������
    private SlotInfo currentItemSlot; // �ش� �������� ����ִ� ����

    private static Tool _instance;
    public static Tool Instance;

    public WeaponType currentWeaponType;

    private Component currentWeaponComponent;
    [SerializeField] SpriteRenderer sprite;

    // �ϴ� ���콺 ������ �ϸ� �帱�� �ٲ�޶�� �Ҹ��� ������
    // ���콺 ������ Ŭ���� �ϸ� ù��°�� �帱�� �ڵ� ������ �ǰ� ��������. �Դٰ� �װ��� ���� ���õ� �͵� ��� ǥ�ø� �ؾ� �ǰ�.
    private void Awake()
    {
        if(_instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject); // �ߺ� ����
        }

        sprite = GetComponent<SpriteRenderer>();
    }

    public void Equip(SlotInfo newSlot)
    {
        RemoveCurrentWeaponComponent();

        // ���� �ν��Ͻ� �켱 �� ������ ������ �ν��Ͻ�
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
            return; // �ν��Ͻ��� �ƹ��͵� ����
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
            // ó�� �� ���� ���ε��ϵ��� üũ
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
            Debug.LogWarning("��� ������ �������� �����ϴ�.");
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
                Debug.LogWarning("�������� �ʴ� ������ Ÿ���Դϴ�.");
                return;
        }

        if(prefab != null)
        {
            GameObject clone = Instantiate(prefab, itemSpawnParent);
            clone.transform.position = transform.position; // �÷��̾� ��ġ���� ���� (���ϸ� ���� ����)
        }

        // ���� ����
        currentItemInstance._count--;

        // UI ����
        SlotManager.Instance.UpdateSlotUI(currentItemSlot, currentItemInstance._item);
    }


    // ���� ����
    public void Unequip()
    {
        sprite.sprite = null;
        RemoveCurrentWeaponComponent();
    }

    // ���� ��������Ʈ ����
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

    // Ÿ�Կ� �´� ���� ������ �ֱ�
    public void ReplaceToolComponent<T>(T instance)
    {
        RemoveCurrentWeaponComponent();

        if(instance == null) return;

        Component toolComponent = null;

        // ���� Ÿ�� ó��
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

        // ���� ������ ���� ������Ʈ�� ���
        currentWeaponComponent = toolComponent;
    }

    // ���� ���� ������Ʈ ����
    public void RemoveCurrentWeaponComponent()
    {
        if(currentWeaponComponent != null)
        {
            Destroy(currentWeaponComponent);
            currentWeaponComponent = null;
        }
    }
}
