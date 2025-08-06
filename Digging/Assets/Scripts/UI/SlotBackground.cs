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
        // Ŭ�� ���� ����: ���� ��� ���� �Ķ���
        img.color = isSelected ? new Color(0.5f, 0.7f, 1f, 0.8f) : defaultColor;
    }

    public void SetEquipped(bool isEquipped)
    {
        // ���� ���� ����: ���� ��� �ʷϻ�
        img.color = isEquipped ? new Color(0.5f, 1f, 0.5f, 0.8f) : defaultColor;
    }
}
