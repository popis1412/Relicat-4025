using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public int count = 0;
    public bool isMineral;
    public int rarity;
    public int value;
    public int duplicate_value;
    public bool ishaveitem;
    public bool isalreadySell;
    public string Info;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!isMineral)
        {
            switch (rarity)
            {
                case 1:
                    value = 100;
                    duplicate_value = value / 5;
                    break;
                case 2:
                    value = 200;
                    duplicate_value = value / 5;
                    break;
                case 3:
                    value = 300;
                    duplicate_value = value / 5;
                    break;
                case 4:
                    value = 400;
                    duplicate_value = value / 5;
                    break;
                case 5:
                    value = 500;
                    duplicate_value = value / 5;
                    break;
            }
        }

    }
#endif
}
