using UnityEngine;

[System.Serializable]
public class WeaponInstance
{
    #region Field
    public WeaponTemplate _template;

    public string _id;
    public float _damage;
    public int _level;
    public float _energy; // 드릴만 필요
    public Vector2 _range;

    public Sprite itemImage;

    #endregion // Field

    #region Method
    public WeaponInstance(WeaponTemplate template, int index)
    {
        _template = template;
        _id = $"{template.name}_{index:D2}";

        _level = 1;
        _damage = template.damage;
        _energy = template.type == WeaponType.Drill ? 100 : -1;
        _range = new Vector2(1, 1);

        itemImage = template.icon;
    }

    // [무기] 
    public Sprite GetSprite()
    {
        if(_template != null)
        {
            return _template.GetSpriteForLevel(_level);
        }
            

        return null;
    }
    #endregion // Method

}
