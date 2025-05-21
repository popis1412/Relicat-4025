using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHintUI : MonoBehaviour
{
    public GameObject hintUI; // "F: ��ȣ�ۿ�" UI ������Ʈ

    private void Start()
    {
        if (hintUI != null)
        {
            hintUI.SetActive(false); // ó���� ����
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (hintUI != null)
                hintUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (hintUI != null)
                hintUI.SetActive(false);
        }
    }
}
