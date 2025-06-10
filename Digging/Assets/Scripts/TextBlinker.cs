using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextBlinker : MonoBehaviour
{
    public TextMeshProUGUI targetText; 
    public float blinkSpeed = 1.0f;  // ±ôºýÀÌ´Â ¼Óµµ (°ªÀÌ Å¬¼ö·Ï ºü¸£°Ô ±ôºýÀÓ)

    private bool fadingOut = true;
    private Color originalColor;

    void Start()
    {
        if (targetText == null)
        {
            targetText = GetComponent<TextMeshProUGUI>();
        }

        originalColor = targetText.color;
    }

    void Update()
    {
        if(LevelManager.instance.isRunning == false)
        {
            Color color = targetText.color;
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

            targetText.color = color;
        }
        
    }
}
