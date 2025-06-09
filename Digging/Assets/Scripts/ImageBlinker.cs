using UnityEngine;
using UnityEngine.UI;

public class ImageBlinker : MonoBehaviour
{
    public Image targetImage;        // �����̰� ���� �̹���
    public float blinkSpeed = 1.0f;  // �����̴� �ӵ� (���� Ŭ���� ������ ������)

    private bool fadingOut = true;
    private Color originalColor;

    void Start()
    {
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }

        originalColor = targetImage.color;
    }

    void Update()
    {
        Color color = targetImage.color;
        float alphaChange = blinkSpeed * Time.deltaTime;

        if (fadingOut)
        {
            color.a -= alphaChange;
            if (color.a <= 0.3f)
            {
                color.a = 0.3f;
                fadingOut = false;
            }
        }
        else
        {
            color.a += alphaChange;
            if (color.a >= originalColor.a)
            {
                color.a = originalColor.a;
                fadingOut = true;
            }
        }

        targetImage.color = color;
    }
}
