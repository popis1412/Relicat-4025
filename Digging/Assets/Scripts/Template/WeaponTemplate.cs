
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
    [Header("���� ����")]
    public string id;
    public string name;
    public WeaponType type;
    public Sprite icon;
    public List<LevelSprite> levelSprite;

    [Header("����(RunTime)")]
    public float damage;
    public int level;
    public Vector2 range;
    public float energy;    // �帱�� �ʿ�
    #endregion // Field

    #region Method

    // ��������Ʈ �̹���
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
