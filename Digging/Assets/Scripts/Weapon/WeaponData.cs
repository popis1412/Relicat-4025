using UnityEngine;

namespace Assets.Scripts.Weapon
{
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapon/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        public string weaponName;
        public float digSpeed;
        public float rangeX;
        public float rangeY;
        public int energy; 
    }
}