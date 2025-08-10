using System;
using System.Collections;
using System.Collections.Generic;
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
    public SlotType _type = SlotType.Null;

    public WeaponInstance _instanceW = null;
    public ItemInstance _instanceI = null;

    public void Initialize(SlotType type, int index)
    {
        _index = index;
        _type = type;
        _instanceW = null;
        _instanceI = null;
    }

    public SlotInfoData ToData()
    {
        return new SlotInfoData(_index, _type, _instanceW, _instanceI);
    }

    public void RestoreFromData(SlotInfoData data)
    {
        _index = data.index;
        _type = data.type;
        _instanceW = data.instanceW;
        _instanceI = data.instanceI;
    }
}
