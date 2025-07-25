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

    [Header("���� ������ ���� �̹��� Ÿ��")]
    public ToolImage currentToolImage;

    SpriteRenderer spriteRenderer;
    private WeaponBase currentWeapon;

    // �̹�����
    [SerializeField] Sprite torchSprite;
    [SerializeField] Sprite bombSprite;
    [SerializeField] Sprite drillSprite;
    [SerializeField] Sprite pickaxeSprite;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // �ʱ� ���� ���� ����
        ChangeTool(ToolImage.Pickaxe);
    }
    public void ChangeTool(ToolImage toolType)
    {
        currentToolImage = toolType;

        // ���� ��������Ʈ ����
        ChangeSprite(toolType);

        // ���� ���� ������Ʈ ��ü
        ChangeWeaponComponent(toolType);
    }

    // �̹��� ����
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

    // �̹����� �´� ���� ������Ʈ �߰�
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
                return; // �������� ���� ������Ʈ �ʿ� ����
        }

        currentWeapon.WeaponData = data;
    }

    private WeaponData GetWeaponData(ToolImage tool)
    {
        // ����: Resources �������� �ε�
        return Resources.Load<WeaponData>($"WeaponData/{tool}");
    }

    public WeaponBase GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
