using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHintUI : MonoBehaviour
{
    public GameObject hintUI_Image; // "F: ��ȣ�ۿ�" UI ������Ʈ
    public GameObject hintUI_Text; // "F: ��ȣ�ۿ�" UI ������Ʈ

    private void Start()
    {
        if (hintUI_Image != null)
        {
            hintUI_Image.SetActive(false); // ó���� ����
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
