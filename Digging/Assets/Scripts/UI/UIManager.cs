using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIManager : MonoBehaviour //Singleton<UIManager>
{
    [SerializeField] TextMeshProUGUI depth;
    [SerializeField] public GameObject DiePanel;
    [SerializeField] public Slider hpBar;

    private void Start()
    {
        depth = GameObject.Find("DepthUI").GetComponent<TextMeshProUGUI>();
        hpBar = GameObject.Find("HPBar").GetComponent<Slider>();

        depth.gameObject.SetActive(false);
        hpBar.gameObject.SetActive(false);
        hpBar.value = 1;
    }

    public TextMeshProUGUI GetTextMeshPro()
    {
        return depth;
    }

    public void SetText(string text)
    {
        depth.text = text;
    }

    public void Invalid(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void Valid(GameObject obj)
    {
        obj.SetActive(true);
    }

    // HP Bar Value 변화
    public void UpdateHP(float hp)
    {
        hp = Mathf.Round((hp / 100) * 100) / 100f;  // 둘째자리까지 나오게 하기
        hpBar.value = hp;
    }
}
