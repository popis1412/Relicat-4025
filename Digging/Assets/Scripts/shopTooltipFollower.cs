using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections.ObjectModel;

public class shopTooltipFollower : MonoBehaviour
{
    public Shop shop;

    public GameObject tooltipPanel; // 따라다닐 UI 패널
    public GameObject drill_tooltipPanel;
    public Vector2 offset = new Vector2(20f, -20f); // 마우스에서의 오프셋

    private bool isHovering = false;

    [SerializeField] private TextMeshProUGUI[] tooltipText_list;
    [SerializeField] private TextMeshProUGUI[] drill_tooltipText_list;



    void Update()
    {
        if(isHovering && tooltipPanel.activeSelf)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                tooltipPanel.transform.parent as RectTransform,
                Input.mousePosition,
                null, // 또는 UI가 World Space가 아니라면 Camera 대신 null
                out pos
            );

            tooltipPanel.GetComponent<RectTransform>().anchoredPosition = pos + offset;


        }
        if(isHovering && drill_tooltipPanel.activeSelf)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                drill_tooltipPanel.transform.parent as RectTransform,
                Input.mousePosition,
                null, // 또는 UI가 World Space가 아니라면 Camera 대신 null
                out pos
            );

            drill_tooltipPanel.GetComponent<RectTransform>().anchoredPosition = pos + offset;


        }
    }

    public void OnPointerEnter(int idx)
    {

        isHovering = true;
        tooltipPanel.SetActive(true);

        switch(shop.shopView_idx)
        {
            case 0:
                tooltipText_list[0].text = shop.player.minerals[idx].itemName;
                tooltipText_list[1].text = "판매 가격 : " + shop.player.minerals[idx].value.ToString() + "원";
                tooltipText_list[2].text = shop.player.minerals[idx].Info;
                break;
            case 1:
                tooltipText_list[0].text = shop.player.UseItems[idx].itemName;
                tooltipText_list[1].text = "구매 가격 : " + shop.player.UseItems[idx].value.ToString() + "원";
                tooltipText_list[2].text = shop.player.UseItems[idx].Info;
                break;
            case 2:
                tooltipText_list[0].text = shop.player.UpgradeItems[idx].itemName;
                tooltipText_list[1].text = "강화 비용 : " + shop.player.UpgradeItems[idx].value.ToString() + "원";
                tooltipText_list[2].text = shop.player.UpgradeItems[idx].Info;
                break;
        }

    }

    public void OnPointerExit()
    {
        isHovering = false;
        tooltipPanel.SetActive(false);
    }

    public void OnPointerEnter_Drill()
    {
        isHovering = true;
        drill_tooltipPanel.SetActive(true);

        if(shop.isCreateDrill)
        {
            drill_tooltipText_list[0].text = shop.player.Drill_Items[0].itemName;
            drill_tooltipText_list[1].text = "강화 비용 : ";
            drill_tooltipText_list[2].text = "X 1";
            drill_tooltipText_list[3].text = "X 2";
            drill_tooltipText_list[4].text = "X 1";
            drill_tooltipText_list[5].text = shop.player.Drill_Items[0].Info;
        }
        else
        {
            drill_tooltipText_list[0].text = shop.player.Drill_Items[0].itemName;
            drill_tooltipText_list[1].text = "제작 재료 : ";
            drill_tooltipText_list[2].text = "X 1";
            drill_tooltipText_list[3].text = "X 5";
            drill_tooltipText_list[4].text = "X 2";
            drill_tooltipText_list[5].text = shop.player.Drill_Items[0].Info;
        }

    }
    public void OnPointerExit_Drill()
    {
        isHovering = false;
        drill_tooltipPanel.SetActive(false);
    }
}
