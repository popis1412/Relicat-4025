using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public Inventory inventory;

    public Collection collection;

    public Shop shop;

    public Player player;

    public PlayerController playerController;

    public LoadScene loadSceneScript;

    public static SaveSystem Instance;
    private string savePath => Application.persistentDataPath + "/SaveData.json";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Save()
    {
        if (inventory == null)
        {
            inventory = FindObjectOfType<Inventory>();
            if (inventory == null)
            {
                print("�κ��丮�� ������ �� ���� ���̺꿡 �����Ͽ����ϴ�");
                return;
            }
        }

        if (collection == null)
        {
            collection = FindObjectOfType<Collection>();
            if (collection == null)
            {
                print("������ ������ �� ���� ���̺꿡 �����Ͽ����ϴ�");
                return;
            }
        }

        if (shop == null)
        {
            shop = FindObjectOfType<Shop>();
            if (shop == null)
            {
                print("������ ������ �� ���� ���̺꿡 �����Ͽ����ϴ�");
                return;
            }
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if (player == null)
            {
                print("�÷��̾ ������ �� ���� ���̺꿡 �����Ͽ����ϴ�");
                return;
            }
        }

        if (playerController == null)
        {
            playerController= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (playerController == null)
            {
                print("�÷��̾� ��Ʈ�ѷ��� ������ �� ���� ���̺꿡 �����Ͽ����ϴ�");
                return;
            }
        }

        if (loadSceneScript == null)
        {
            loadSceneScript = FindObjectOfType<LoadScene>();
            if (loadSceneScript == null)
            {
                print("�ε���� ������ �� ���� ���̺꿡 �����Ͽ����ϴ�");
                return;
            }
        }

        var saveData = new SaveData();


        //�κ��丮 ����
        saveData.inventoryData = new InventoryData
        {
            items = ConvertItemList(inventory.items),
            money_item = ConvertItem(inventory.money_item)
        };

        //���� ����
        saveData.collectionData = new CollectionData
        {
            collect_sum = collection.collect_sum,
            player_lv = collection.player_lv,
            li_isCollect = collection.li_isCollect,
            li_isRelicOnTable = collection.li_isRelicOnTable
        };

        //���� ����
        saveData.shopData = new ShopData
        {
            pick_damage = shop.pick_damage,
            lightRadius = shop.lightRadius
        };

        //�÷��̾� ����
        saveData.playerData = new PlayerData
        {
            items = ConvertItemList(player.items),
            minerals = ConvertItemList(player.minerals),
            UseItems = ConvertItemList(player.UseItems),
            UpgradeItems = ConvertItemList(player.UpgradeItems)
        };

        //�÷��̾���Ʈ�ѷ� ����
        saveData.playerControllerData = new PlayerControllerData
        {
            pickDamage = playerController.pickdamage
        };

        //�ε�������� ����
        saveData.loadSceneData = new LoadSceneData
        {
            isAlreadyWatchStory = loadSceneScript.isAlreadyWatchStory
        };


        //������ ������ json���·� ���� �� ������ ������ savePath��λ� ����
        string jsonForSave = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, jsonForSave);
        print("����Ϸ�");

    }

    public void Load()
    {
        if(!File.Exists(savePath))
        {
            print("����� ������ �����ϴ�");
            return;
        }

        if (inventory == null)
        {
            inventory = FindObjectOfType<Inventory>();
            if (inventory == null)
            {
                print("�κ��丮�� ������ �� ���� �ε忡 �����Ͽ����ϴ�");
                return;
            }
        }

        if (collection == null)
        {
            collection = FindObjectOfType<Collection>();
            if (collection == null)
            {
                print("������ ������ �� ���� �ε忡 �����Ͽ����ϴ�");
                return;
            }
        }

        if (shop == null)
        {
            shop = FindObjectOfType<Shop>();
            if (shop == null)
            {
                print("������ ������ �� ���� �ε忡 �����Ͽ����ϴ�");
                return;
            }
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if (player == null)
            {
                print("�÷��̾ ������ �� ���� �ε忡 �����Ͽ����ϴ�");
                return;
            }
        }

        if (playerController == null)
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (playerController == null)
            {
                print("�÷��̾� ��Ʈ�ѷ��� ������ �� ���� �ε忡 �����Ͽ����ϴ�");
                return;
            }
        }

        if (loadSceneScript == null)
        {
            loadSceneScript = FindObjectOfType<LoadScene>();
            if (loadSceneScript == null)
            {
                print("�ε���� ������ �� ���� �ε忡 �����Ͽ����ϴ�");
                return;
            }
        }


        //�ε� ����
        string jsonForLoad = File.ReadAllText(savePath);
        SaveData loaded = JsonUtility.FromJson<SaveData>(jsonForLoad);

        //�κ��丮 �ε�
        inventory.items = CreateItemList(loaded.inventoryData.items);
        ApplyItem(inventory.money_item, loaded.inventoryData.money_item);

        //�÷��̾� �ε�
        ApplyItemList(player.items, loaded.playerData.items);
        ApplyItemList(player.minerals, loaded.playerData.minerals);
        ApplyItemList(player.UseItems, loaded.playerData.UseItems);
        ApplyItemList(player.UpgradeItems, loaded.playerData.UpgradeItems);

        //���� �ε�
        collection.collect_sum = loaded.collectionData.collect_sum;
        collection.player_lv = loaded.collectionData.player_lv;
        collection.li_isCollect = loaded.collectionData.li_isCollect;
        collection.li_isRelicOnTable = loaded.collectionData.li_isRelicOnTable;

        //���� �ε�
        shop.pick_damage = loaded.shopData.pick_damage;
        shop.lightRadius = loaded.shopData.lightRadius;

        //�÷��̾���Ʈ�ѷ� �ε�
        playerController.pickdamage = loaded.playerControllerData.pickDamage;

        //�ε�� �ε�
        loadSceneScript.isAlreadyWatchStory = loaded.loadSceneData.isAlreadyWatchStory;

        //UI�簻��(�Ƹ��� ������ ���� �־���� ����)
        inventory.FreshSlot();
        print("�ε�Ϸ�");



    }

    public void LoadForLoadScene()
    {
        if (!File.Exists(savePath))
        {
            print("����� ������ �����ϴ�");
            return;
        }


        if (loadSceneScript == null)
        {
            loadSceneScript = FindObjectOfType<LoadScene>();
            if (loadSceneScript == null)
            {
                print("�ε���� ������ �� ���� �ε忡 �����Ͽ����ϴ�");
                return;
            }
        }


        //�ε� ����
        string jsonForLoad = File.ReadAllText(savePath);
        SaveData loaded = JsonUtility.FromJson<SaveData>(jsonForLoad);

        loadSceneScript.isAlreadyWatchStory = loaded.loadSceneData.isAlreadyWatchStory;
    }



    public void DeleteSaveFile()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            print("���̺� ������ �����Ǿ����ϴ�.");
        }
        else
        {
            print("���̺� ������ �������� �ʽ��ϴ�.");
        }
    }


    private List<ItemData> ConvertItemList(List<Item> itemList)
    {
        var result = new List<ItemData>();

        foreach(var item in itemList)
        {
            result.Add(ConvertItem(item));
        }

        return result;
    }
    
    private ItemData ConvertItem(Item item)
    {
        var result = new ItemData();

        result.itemName = item.itemName;
        result.count = item.count;
        result.accumulation_count = item.accumulation_count;
        result.isMineral = item.isMineral;
        result.isRelic = item.isRelic;
        result.value = item.value;
        result.ishaveitem = item.ishaveitem;
        result.isalreadySell = item.isalreadySell;

        return result;
    }

    private void ApplyItemList(List<Item> itemList, List<ItemData> dataList)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            ApplyItem(itemList[i], dataList[i]);
        }
    }

    private void ApplyItem(Item item, ItemData data)
    {
        item.count = data.count;
        item.accumulation_count = data.accumulation_count;
        item.value = data.value;
        item.ishaveitem = data.ishaveitem;
        item.isalreadySell = data.isalreadySell;
    }

    private List<Item> CreateItemList(List<ItemData> dataList)
    {
        var result = new List<Item>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        if(player == null)
        {
            print("�÷��̾ ���� ������ ������ �ҷ��� �� �����ϴ�.");
        }
        else
        {
            foreach (var data in dataList)
            {
                if (data.isMineral)
                {
                    for(int i = 0; i < player.minerals.Count; i++)
                    {
                        if (data.itemName == player.minerals[i].itemName)
                        {
                            result.Add(player.minerals[i]);
                            break;
                        }
                    }
                }
                else if(data.isRelic)
                {
                    for (int i = 0; i < player.items.Count; i++)
                    {
                        if (data.itemName == player.items[i].itemName)
                        {
                            result.Add(player.items[i]);
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < player.UseItems.Count; i++)
                    {
                        if (data.itemName == player.UseItems[i].itemName)
                        {
                            result.Add(player.UseItems[i]);
                            break;
                        }
                    }
                }
            }
        }

        return result;
    }
}
