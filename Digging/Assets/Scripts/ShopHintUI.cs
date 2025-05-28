using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHintUI : MonoBehaviour
{
    public GameObject hintUI_Image; // "F: 상호작용" UI 오브젝트
    public GameObject hintUI_Text; // "F: 상호작용" UI 오브젝트

    private void Start()
    {
        if (hintUI_Image != null)
        {
            hintUI_Image.SetActive(false); // 처음엔 숨김
            hintUI_Text.SetActive(false);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (hintUI_Image != null)
                hintUI_Image.SetActive(true);
                hintUI_Text.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (hintUI_Image != null)
                hintUI_Image.SetActive(false);
                hintUI_Text.SetActive(false);
        }
    }
}
