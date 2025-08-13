using UnityEngine;

public enum SlotType
{
    Null,
    QuickSlot,
    Inventory
}

public class SlotInfo : MonoBehaviour
{
    public int _index;
    public SlotType _typeS;
    public ItemType _typeI;
    public WeaponType _typeW;

    public WeaponInstance _instanceW = null;
    public ItemInstance _instanceI = null;

    public void Initialize(SlotType type, int index)
    {
        _index = index;
        _typeS = type;
        _instanceW = null;
        _instanceI = null;

        // 인덱스에 따라서 무기와 아이템 타입 지정하기(해당 지정석에만 아이템 채우기)
        switch(index)
        {
            case 0:
                _typeW = WeaponType.Pickaxe;
                _typeI = ItemType.Null;
                break;
            case 1:
                _typeW = WeaponType.Null;
                _typeI = ItemType.Bomb;
                break;
            case 2:
                _typeW = WeaponType.Null;
                _typeI = ItemType.Torch;
                break;
            case 3:
                _typeW = WeaponType.Null;
                _typeI = ItemType.Teleport;
                break;
            case 4:
                _typeW = WeaponType.Drill;
                _typeI = ItemType.Null;
                break;
            default:
                _typeW = WeaponType.Null;
                _typeI = ItemType.Null;
                break;
        }
    }

    public SlotInfoData ToData()
    {
        return new SlotInfoData(_index, _typeS, _instanceW, _instanceI);
    }

    public void RestoreFromData(SlotInfoData data)
    {
        _index = data.index;
        _typeS = data.type;
        _instanceW = data.instanceW;
        _instanceI = data.instanceI;
    }
}
