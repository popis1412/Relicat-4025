using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public List<Vector2> torchPositions = new List<Vector2>();
    [SerializeField] private GameObject bombPrefab;
    public GameObject torchPrefab;
    [SerializeField] private Vector2 itemSpawnParent;

    private ItemInstance currentItemInstance; // ���� ������ ������
    private SlotInfo currentItemSlot; // �ش� �������� ����ִ� ����

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

    public WeaponType currentWeaponType;

    private Component currentWeaponComponent;
    [SerializeField] private SpriteRenderer sprite;

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

        sprite = GetComponent<SpriteRenderer>();
    }

    // ���� ���� + ǥ��
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

    // ���� ���
    public void UseWeapon(Vector2 mousePos, Player player)
    {
        if(currentWeaponComponent == null)
            return;

        PlayerController _player = player.GetComponent<PlayerController>();
        bool isSanded = _player.ISInSand;   // �� ���� ����

        if(currentWeaponComponent is Pickaxe pickaxe)
        {
            pickaxe.Digging(mousePos, player, isSanded);
        }
        else if(currentWeaponComponent is Drill drill)
        {
            drill.Digging(mousePos, player, isSanded);
        }
    }

    // ������ ���
    public void UseItem(bool isGrounded)
    {
        itemSpawnParent = GameObject.FindGameObjectWithTag("Player").transform.position;

        if(currentItemInstance == null || currentItemInstance._item.count <= 0)
        {
            Debug.LogWarning("��� ������ �������� �����ϴ�.");
            return;
        }

        GameObject prefab = null;

        switch(currentItemInstance._item.type)
        {
            case ItemType.Bomb:
                if(!isGrounded)
                {
                    print("��ź�� ���߿��� ��ġ�� �� �����ϴ�.");
                    return;
                }
                prefab = bombPrefab;
                
                break;
            case ItemType.Torch:
                if(!isGrounded)
                {
                    print("ȶ���� ���߿��� ��ġ�� �� �����ϴ�.");
                    return;
                }
                prefab = torchPrefab;
                
                // Ƚ�� ��ġ �� �÷��̾��� �ǹ��� (0,0) ��ġ�� ��ġ -> ���߿� ���� �߾� ��ġ�� ��ġ�ϰ� �� ����.
                SpriteRenderer sr = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
                float playerSize = sr.size.y;
                float torchSize = torchPrefab.GetComponent<SpriteRenderer>().size.y;
                itemSpawnParent = new Vector3(Mathf.Round(transform.position.x - 0.5f) + 0.5f, transform.position.y - (playerSize - torchSize));
                break;
            case ItemType.Teleport:
                if(!isGrounded)
                {
                    print("�ڷ���Ʈ�� ���߿��� ����� �� �����ϴ�");
                    return;
                }
                Teleport teleport = new Teleport();
                teleport.Spawn(transform.parent.GetComponent<PlayerController>());
                break;

            default:
                Debug.LogWarning("�������� �ʴ� ������ Ÿ���Դϴ�.");
                return;
        }

        if(prefab != null)
        {
            GameObject clone = Instantiate(prefab, itemSpawnParent, Quaternion.identity);
            clone.transform.position = transform.position; // �÷��̾� ��ġ���� ���� (���ϸ� ���� ����)
            clone.transform.SetParent(null); // �θ� �������� ���� ���� �ʱ�

            if(prefab == torchPrefab)
                torchPositions.Add(itemSpawnParent);
        }

        // ���� ����
        currentItemInstance._item.count--;
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
            sprite.sprite = weapon == null ? null : weapon.GetSprite();
        }
        else if(instance is ItemInstance item)
        {
            sprite.sprite = item == null ? null: item.GetSprite();
        }
    }

    // Ÿ�Կ� �´� ���� ������ �ֱ�
    public void ReplaceToolComponent<T>(T instance)
    {
        

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

        RemoveCurrentWeaponComponent();

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
