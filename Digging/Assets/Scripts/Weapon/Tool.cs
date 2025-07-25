using Assets.Scripts.Weapon;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public enum ToolType { Weapon, Item };
    public enum ToolImage
    {
        Bomb,
        Torch,
        Drill,
        Pickaxe
    }

    [Header("현재 장착된 무기 이미지 타입")]
    public ToolImage currentToolImage;

    SpriteRenderer spriteRenderer;
    private WeaponBase currentWeapon;

    // 이미지들
    [SerializeField] Sprite torchSprite;
    [SerializeField] Sprite bombSprite;
    [SerializeField] Sprite drillSprite;
    [SerializeField] Sprite pickaxeSprite;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 초기 장착 무기 설정
        ChangeTool(ToolImage.Pickaxe);
    }
    public void ChangeTool(ToolImage toolType)
    {
        currentToolImage = toolType;

        // 무기 스프라이트 설정
        ChangeSprite(toolType);

        // 무기 로직 컴포넌트 교체
        ChangeWeaponComponent(toolType);
    }

    // 이미지 변경
    private void ChangeSprite(ToolImage tool)
    {
        switch(tool)
        {
            case ToolImage.Torch:
                spriteRenderer.sprite = torchSprite;
                break;
            case ToolImage.Bomb:
                spriteRenderer.sprite = bombSprite;
                break;
            case ToolImage.Drill:
                spriteRenderer.sprite = drillSprite;
                break;
            case ToolImage.Pickaxe:
                spriteRenderer.sprite = pickaxeSprite;
                break;
        }
    }

    // 이미지에 맞는 무기 컴포넌트 추가
    private void ChangeWeaponComponent(ToolImage tool)
    {
        if(currentWeapon != null)
            Destroy(currentWeapon);

        WeaponData data = GetWeaponData(tool);

        switch(tool)
        {
            case ToolImage.Drill:
                currentWeapon = gameObject.AddComponent<Drill>();
                break;
            case ToolImage.Pickaxe:
                currentWeapon = gameObject.AddComponent<Pickaxe>();
                break;
            default:
                return; // 아이템은 무기 컴포넌트 필요 없음
        }

        currentWeapon.WeaponData = data;
    }

    private WeaponData GetWeaponData(ToolImage tool)
    {
        // 예시: Resources 폴더에서 로드
        return Resources.Load<WeaponData>($"WeaponData/{tool}");
    }

    public WeaponBase GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
