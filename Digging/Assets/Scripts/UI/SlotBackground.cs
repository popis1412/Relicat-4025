using UnityEngine;
using UnityEngine.UI;

public class SlotBackground : MonoBehaviour
{
    [SerializeField] private Image img;

    Color defaultColor = new Color(0, 0, 0, 0);

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    public void SetSelected(bool isSelected)
    {
        // 클릭 강조 색상: 예를 들어 연한 파란색
        img.color = isSelected ? new Color(0.5f, 0.7f, 1f, 0.8f) : defaultColor;
    }

    public void SetEquipped(bool isEquipped)
    {
        // 무기 착용 강조: 예를 들어 초록색
        img.color = isEquipped ? new Color(0.5f, 1f, 0.5f, 0.8f) : defaultColor;
    }
}
