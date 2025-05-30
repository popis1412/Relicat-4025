using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections.ObjectModel;

public class shopTooltipFollower : MonoBehaviour
{
    public Shop shop;

    public GameObject tooltipPanel; // ����ٴ� UI �г�
    public Vector2 offset = new Vector2(20f, -20f); // ���콺������ ������

    private bool isHovering = false;

    [SerializeField] private TextMeshProUGUI[] tooltipText_list;



    void Update()
    {
        if (isHovering && tooltipPanel.activeSelf)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                tooltipPanel.transform.parent as RectTransform,
                Input.mousePosition,
                null, // �Ǵ� UI�� World Space�� �ƴ϶�� Camera ��� null
                out pos
            );

            tooltipPanel.GetComponent<RectTransform>().anchoredPosition = pos + offset;


        }
    }

    public void OnPointerEnter(int idx)
    {
        
        isHovering = true;
        tooltipPanel.SetActive(true);
        
        switch (shop.shopView_idx)
        {
            case 0:
                tooltipText_list[0].text = shop.player.minerals[idx].itemName;
                tooltipText_list[1].text = "�Ǹ� ���� : " + shop.player.minerals[idx].value.ToString() + "��";
                tooltipText_list[2].text = shop.player.minerals[idx].Info;
                break;
            case 1:
                tooltipText_list[0].text = shop.player.UseItems[idx].itemName;
                tooltipText_list[1].text = "���� ���� : " + shop.player.UseItems[idx].value.ToString() + "��";
                tooltipText_list[2].text = shop.player.UseItems[idx].Info;
                break;
            case 2:
                tooltipText_list[0].text = shop.player.UpgradeItems[idx].itemName;
                tooltipText_list[1].text = "��ȭ ��� : " + shop.player.UpgradeItems[idx].value.ToString() + "��";
                tooltipText_list[2].text = shop.player.UpgradeItems[idx].Info;
                break;
        }

    }

    public void OnPointerExit()
    {
        isHovering = false;
        tooltipPanel.SetActive(false);
    }
}
