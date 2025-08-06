
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Pickaxe,
    Drill
}

[System.Serializable]
public class LevelSprite
{
    public int level;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "Template", menuName = "Inventory/Weapon Template")]
public class WeaponTemplate : ScriptableObject
{
    #region Field
    [Header("공통 정보")]
    public string id;
    public string name;
    public WeaponType type;
    public Sprite icon;
    public List<LevelSprite> levelSprite;

    [Header("스탯(RunTime)")]
    public float damage;
    public int level;
    public Vector2 range;
    public float energy;    // 드릴만 필요
    #endregion // Field

    #region Method

    // 스프라이트 이미지
    public Sprite GetSpriteForLevel(int level)
    {
        Sprite result = icon;

        foreach(var entry in levelSprite)
            if(level >= entry.level)
                result = entry.sprite;

        return result;
    }

    #endregion // Method

}
