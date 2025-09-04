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
        }
        else if(instanceI != null)
        {
            SetWeaponSprite(instanceI);
            ReplaceToolComponent(instanceI); // T = ItemInstance
        }
    }

    // ���� ���
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
                    // ó�� �� ���� ���ε��ϵ��� üũ
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
                    // ó�� �� ���� ���ε��ϵ��� üũ
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

        // ������ �� ����
        itemInstance._count--;
        // UI �ؽ�Ʈ ����
        SlotManager.Instance.UpdateSlotUI(slot, slot._instanceI._item);

        // ���� 0 ������ ��� ���� ���
        if(itemInstance._count <= 0)
        {
            slot.GetComponentInChildren<SlotInteraction>()?.Clear();
        }

        // ������ ã��
        GameObject prefab = null;

        switch(itemInstance._item.type)
        {
            case ItemType.Bomb:
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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
                if(!isGrounded)
                {
                    print("�ڷ���Ʈ�� ���߿��� ����� �� �����ϴ�");
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
                Debug.LogWarning("�������� �ʴ� ������ Ÿ���Դϴ�.");
                return;
        }

        // ������ Ŭ�� ����
        if(prefab != null && spawnTransform != null)
        {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            GameObject clone = Instantiate(prefab, itemSpawnParent, Quaternion.identity);
            clone.transform.position = transform.position; // �÷��̾� ��ġ���� ���� (���ϸ� ���� ����)
            clone.transform.SetParent(null); // �θ� �������� ���� ���� �ʱ�

            if(prefab == torchPrefab)
                torchObj.Add(clone);
        }

        // ���� ����
        currentItemInstance._item.count--;
        // UI ����
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
