using Assets.Scripts.Weapon;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField] protected WeaponData weaponData;

    // 런타임 값: 게임 중 변경되는 상태
    [SerializeField] protected float _digSpeed;
    [SerializeField] protected float _rangeX;
    [SerializeField] protected float _rangeY;
    [SerializeField] protected int _energy;
    
    public float DigSpeed
    { 
        get => _digSpeed; 
        set => _digSpeed = value; 
    }
    public float RangeX
    {
        get => _rangeX;
        set => _rangeX = value;
    }
    public float RangeY
    {
        get => _rangeY;
        set => _rangeY = value;
    }
    public int Energy
    {
        get => _energy;
        set => _energy = value;
    }

    public WeaponData WeaponData
    {
        get => weaponData;
        set
        {
            weaponData = value;
            if(weaponData != null)
            {
                // weaponData에서 기본값을 런타임 변수에 복사해 초기화
                _digSpeed = weaponData.digSpeed;
                _rangeX = weaponData.rangeX;
                _rangeY = weaponData.rangeY;
                _energy = weaponData.energy;
            }
        }
    }

    public virtual void ChargeEnergy()
    {
        weaponData.energy = 100;
    }

    public virtual void Digging(Block target, Player player)
    {
        target.BlockDestroy(_digSpeed * Time.deltaTime, player);
        print($"캐는 속도: {_digSpeed}, 범위X: {_rangeX}, 범위Y: {_rangeY}, 에너지: {_energy}");
    }
}
