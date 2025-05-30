using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public Inventory Inventory;
    public Player player;
    public PlayerController playerController;

    public int shopView_idx;
    [SerializeField] private GameObject[] shopList;

    [SerializeField] private TextMeshProUGUI shop_pickLvText;

    private void Start()
    {
        shopView_idx = 0;
        Switch_ShopView();
    }

    // ���� �̵� ����
    public void Button_Left()
    {
        shopView_idx -= 1;
        if (shopView_idx < 0 )
        {
            shopView_idx = 2;
        }
        Switch_ShopView();
    }

    // ���� �̵� ������
    public void Button_Right()
    {
        shopView_idx += 1;
        if (shopView_idx > 2)
        {
            shopView_idx = 0;
        }
        Switch_ShopView();
    }

    // ���� ���� 0 : �Ǹ� / 1 : ���� / 2 : ���׷��̵�
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


    // ���� �Ǹ� ��ư
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


    //������ ���� ��ư
    public void Button_Buy_Item_Bomb()
    {
        if(Inventory.money >= player.UseItems[0].value)
        {
            Inventory.money -= player.UseItems[0].value;
            Inventory.AddItem(player.UseItems[0], 1);
        }
        
    }
    public void Button_Buy_Item_Torch()
    {
        if (Inventory.money >= player.UseItems[1].value)
        {
            Inventory.money -= player.UseItems[1].value;
            Inventory.AddItem(player.UseItems[1], 1);
        }

    }

    // ���׷��̵� ��ư
    public void Button_Upgrade_Pick()
    {
        if(Inventory.money >= player.UpgradeItems[0].value)
        {
            Inventory.money -= player.UpgradeItems[0].value;
            playerController.pickdamage += 0.4f;
            player.UpgradeItems[0].count++;
            shop_pickLvText.text = "���� : " + player.UpgradeItems[0].count;
            player.UpgradeItems[0].value += 10;

            Debug.Log(playerController.pickdamage);
            Inventory.FreshSlot();
        }
    }

}
