using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public Inventory Inventory;
    public Player player;

    private int shopView_idx;
    [SerializeField] private GameObject[] shopList;

    private void Start()
    {
        shopView_idx = 0;
        Switch_ShopView();
    }

    // 상점 이동 왼쪽
    public void Button_Left()
    {
        shopView_idx -= 1;
        if (shopView_idx < 0 )
        {
            shopView_idx = 2;
        }
        Switch_ShopView();
    }

    // 상점 이동 오른쪽
    public void Button_Right()
    {
        shopView_idx += 1;
        if (shopView_idx > 2)
        {
            shopView_idx = 0;
        }
        Switch_ShopView();
    }

    // 상점 구분 0 : 판매 / 1 : 구매 / 2 : 업그레이드
    private void Switch_ShopView()
    {
        switch (shopView_idx)
        {
            case 0:
                shopList[0].SetActive(true);
                shopList[1].SetActive(false);
                shopList[2].SetActive(false);
                break;
            case 1:
                shopList[0].SetActive(false);
                shopList[1].SetActive(true);
                shopList[2].SetActive(false);
                break;
            case 2:
                shopList[0].SetActive(false);
                shopList[1].SetActive(false);
                shopList[2].SetActive(true);
                break;
        }
    }


    // 광물 판매 버튼
    public void Button_Sell_Mineral_Coal()
    {
        Inventory.SellItem(player.minerals[0]);
    }
    public void Button_Sell_Mineral_Copper()
    {
        Inventory.SellItem(player.minerals[1]);
    }
    public void Button_Sell_Mineral_Iron()
    {
        Inventory.SellItem(player.minerals[2]);
    }
    public void Button_Sell_Mineral_Gold()
    {
        Inventory.SellItem(player.minerals[3]);
    }
    public void Button_Sell_Mineral_Ruby()
    {
        Inventory.SellItem(player.minerals[4]);
    }
    public void Button_Sell_Mineral_Diamond()
    {
        Inventory.SellItem(player.minerals[5]);
    }

    public void Button_All_Sell_Mineral_Coal()
    {
        Inventory.SellAllItem(player.minerals[0]);
    }
}
