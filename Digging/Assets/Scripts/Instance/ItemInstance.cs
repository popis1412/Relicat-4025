using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    #region Field
    public Item _item;
    public int _count;
    public Sprite itemImage;

    public ItemInstance(Item item)
    {
        _item = item;
        itemImage = item.itemImage;
        _count = item.count;
        //_count = item.accumulation_count; // ���߿� ���������̳� ������ ��ȯ�� �ִٸ� �ʿ���.
    }

    public ItemInstance(Item item, int count)
    {
        _item = item;
        itemImage = item.itemImage;
        _count = count;
    }

    public Sprite GetSprite()
    {
        if(_item != null)
            return _item.itemImage;

        return null;
    }

    #endregion Field

}
