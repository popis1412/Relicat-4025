using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    public Inventory Inventory;
    public Player player;
    public PlayerController playerController;

    public int shopView_idx;
    [SerializeField] private GameObject[] shopList;

    [SerializeField] private TextMeshProUGUI shop_pickLvText;
    [SerializeField] private TextMeshProUGUI shop_lightLvText;

    [SerializeField] private Light2D playerlight;

    public float pick_damage;
    public float lightRadius;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.root.gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(this.gameObject); // �ߺ� ����
        }
        pick_damage = playerController.pickdamage;
        lightRadius = playerlight.pointLightOuterRadius;

        Debug.Log(pick_damage);
        Debug.Log(lightRadius);
    }

    private void Start()
    {
        shopView_idx = 0;
        Switch_ShopView();

        
    }

    private void OnDestroy()
    {
        // �̺�Ʈ ���� (�ߺ� ����)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� �ε�� �� Player �ٽ� ã��
        player = FindObjectOfType<Player>();
        playerController = FindObjectOfType<PlayerController>();
        playerlight = FindAnyObjectByType<Light2D>();
        Debug.Log(playerlight.gameObject.name);

        playerController.pickdamage = pick_damage;
        playerlight.pointLightOuterRadius = lightRadius;

        if (player != null)
        {
            Debug.Log("�� ��ȯ �� Player ���� �Ϸ�: " + player.name);
        }
        else
        {
            Debug.LogWarning("�� ��ȯ �� Player�� ã�� ���߽��ϴ�.");
        }
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
        if(Inventory.money_item.count >= player.UseItems[0].value)
        {
            Inventory.money_item.count -= player.UseItems[0].value;
            Inventory.AddItem(player.UseItems[0], 1);
        }
        
    }
    public void Button_Buy_Item_Torch()
    {
        if (Inventory.money_item.count >= player.UseItems[1].value)
        {
            Inventory.money_item.count -= player.UseItems[1].value;
            Inventory.AddItem(player.UseItems[1], 1);
        }

    }

    // ���׷��̵� ��ư
    public void Button_Upgrade_Pick()
    {
        if(Inventory.money_item.count >= player.UpgradeItems[0].value)
        {
            Inventory.money_item.count -= player.UpgradeItems[0].value;
            pick_damage += 0.4f;
            playerController.pickdamage = pick_damage;
            player.UpgradeItems[0].count++;
            shop_pickLvText.text = "���� : " + player.UpgradeItems[0].count;
            player.UpgradeItems[0].value += 10;

            Debug.Log(playerController.pickdamage);
            Inventory.FreshSlot();
        }
    }
    public void Button_Upgrade_EyeLight()
    {
        if (Inventory.money_item.count >= player.UpgradeItems[1].value)
        {
            Inventory.money_item.count -= player.UpgradeItems[1].value;
            lightRadius += 0.1f;
            playerlight.pointLightOuterRadius = lightRadius;
            player.UpgradeItems[1].count++;
            shop_lightLvText.text = "���� : " + player.UpgradeItems[1].count;
            player.UpgradeItems[1].value += 50;

            Debug.Log(playerlight.pointLightOuterRadius);
            Inventory.FreshSlot();
        }
    }

}
