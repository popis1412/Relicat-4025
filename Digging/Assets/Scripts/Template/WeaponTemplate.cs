
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public enum WeaponType
{
    Null,
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

        // ���� �������� ��ȸ�Ͽ� ������ �����ϴ� ù ��������Ʈ�� ã��
        foreach(var entry in levelSprite.OrderByDescending(entry => entry.level))
        {
            if(level >= entry.level)
            {
                result = entry.sprite;
                Debug.Log($"�̹���: {result.name}, ���� ����: {level}, ���� ����: {entry.level}");
                break;
            }
        }


        return result;
    }

    #endregion // Method

}
