using UnityEngine;
using UnityEngine.UI;

public class ImageBlinker : MonoBehaviour
{
    public Image targetImage;        // 깜빡이게 만들 이미지
    public float blinkSpeed = 1.0f;  // 깜빡이는 속도 (값이 클수록 빠르게 깜빡임)

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
