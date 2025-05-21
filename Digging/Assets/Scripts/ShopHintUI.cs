using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHintUI : MonoBehaviour
{
    public GameObject hintUI; // "F: 상호작용" UI 오브젝트

    private void Start()
    {
        if (hintUI != null)
        {
            hintUI.SetActive(false); // 처음엔 숨김
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
