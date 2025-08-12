using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string itemName;
    public int count = 0;
    public int accumulation_count;
    public bool isMineral;
    public bool isRelic;
    public bool isDrillItem;
    public int value;
    public bool ishaveitem;
    public bool isalreadySell;
}

[System.Serializable]
public class InventoryData
{
    public List<ItemData> items;
    public ItemData money_item;
}

[System.Serializable]
public class CollectionData
{
    public int collect_sum;
    public int player_lv;
    public bool[] li_isCollect;
    public bool[] li_isRelicOnTable;
}

[System.Serializable]
public class ShopData
{
    public float pick_damage;
    public float lightRadius;
}


[System.Serializable]
public class PlayerData
{
    public List<ItemData> items;
    public List<ItemData> minerals;
    public List<ItemData> UseItems;
    public List<ItemData> UpgradeItems;
    public List<ItemData> Drill_Items;
}


[System.Serializable]
public class WeaponsData
{
    public List<WeaponsData> ownedWeapons;
}

[System.Serializable]
public class LoadSceneData
{
    public bool isAlreadyWatchStory = false;
    public int stage_Level = 0;
    public int difficulty_level = 0;
}

[System.Serializable]
public class LevelManageData
{
    public float remainingTime;
}


[System.Serializable]
public class BlockData
{
    public Vector3 blockPosition;
    public int nowBlockType;
    public int blockType;
    public int stageNum;
    public float blockHealth;
}

[System.Serializable]
public class BlocksData
{
    public List<BlockData> blockDatas;
}


[System.Serializable]
public class EnemyData
{
    public List<Vector2> EnemyDatas;
}

[System.Serializable]
public class SlotInfoData   // ������ �����͵�
{
    public int index;
    public SlotType type;
    public WeaponInstance instanceW;
    public ItemInstance instanceI;

    public SlotInfoData(int index, SlotType type, WeaponInstance instanceW, ItemInstance instanceI)
    {
        this.index = index;
        this.type = type;
        this.instanceW = instanceW;
        this.instanceI = instanceI;
    }
}

[System.Serializable]
public class QuickSlotSaveData
{
    public List<SlotInfoData> slots = new List<SlotInfoData>();
    public SlotInfoData currentWeapon;  // ������ ���� ������
}

[System.Serializable]
public class SaveData
{
    public InventoryData inventoryData;
    public CollectionData collectionData;
    public ShopData shopData;
    public PlayerData playerData;
    public WeaponsData weaponData;
    public LoadSceneData loadSceneData;
    public LevelManageData levelManageData;
    public BlocksData blocksData;
    public EnemyData enemyData;
    public QuickSlotSaveData quickSlotInfoData;
}