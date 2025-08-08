using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    public Inventory Inventory;
    public Player player;
    public PlayerController playerController;

    public int shopView_idx;
    [SerializeField] private GameObject[] shopList;

    public Image pickImage;
    public TextMeshProUGUI shop_pickLvText;
    public TextMeshProUGUI shop_pickUpdateText;
    public TextMeshProUGUI shop_lightLvText;
    public TextMeshProUGUI shop_lightUpdateText;

    public GameObject playerlight;

    public float pick_damage;
    public float lightRadius;

    // 드릴
    public GameObject createDrill_textobj;
    public GameObject upgradeDrill_textobj;
    public TextMeshProUGUI drill_Lv_Text;
    public bool isCreateDrill;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.root.gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded2;
        }
        else
        {
            Destroy(this.gameObject); // 중복 방지
        }
        pick_damage = playerController.pickdamage;
        lightRadius = playerlight.GetComponent<Light2D>().pointLightOuterRadius;

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
        // 이벤트 제거 (중복 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded2;
    }

    private void OnSceneLoaded2(Scene scene, LoadSceneMode mode)
    {
        //if(scene.buildIndex != 2 || scene.buildIndex != 3) return;

        // 씬이 로드된 후 Player 다시 찾기
        player = FindObjectOfType<Player>();
        playerController = FindObjectOfType<PlayerController>();
        playerlight = GameObject.Find("Spot Light 2D");
        //Debug.Log(playerlight.gameObject.name);

        

        if (player != null)
        {
            Debug.Log("씬 전환 후 Player 연결 완료: " + player.name);
            playerController.pickdamage = pick_damage;
            playerlight.GetComponent<Light2D>().pointLightOuterRadius = lightRadius;
        }
        else
        {
            Debug.Log("씬 전환 후 Player를 찾지 못했습니다.");
        }
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
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[30]);

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
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[30]);

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
        Inventory.SellAllItem(player.minerals[0]);
    }
    public void Button_Sell_Mineral_Copper()
    {
        Inventory.SellAllItem(player.minerals[1]);
    }
    public void Button_Sell_Mineral_Iron()
    {
        Inventory.SellAllItem(player.minerals[2]);
    }
    public void Button_Sell_Mineral_Gold()
    {
        Inventory.SellAllItem(player.minerals[3]);
    }
    public void Button_Sell_Mineral_Ruby()
    {
        Inventory.SellAllItem(player.minerals[4]);
    }
    public void Button_Sell_Mineral_Diamond()
    {
        Inventory.SellAllItem(player.minerals[5]);
    }

    public void Button_All_Sell_Mineral_Coal()
    {
        Inventory.SellAllItem(player.minerals[0]);
    }


    //아이템 구매 버튼
    public void Button_Buy_Item_Bomb()
    {
        if(Inventory.money_item.count >= player.UseItems[0].value)
        {
            //SlotManager.Instance.FillSlot(player.UseItems[0], 1);
            Inventory.money_item.count -= player.UseItems[0].value;
            Inventory.AddItem(player.UseItems[0], 1);
            SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[33]);
        }
        else
        {
            Inventory.LogMessage("돈이 부족합니다");
        }

    }

    public void Button_Buy_Item_Torch()
    {
        if (Inventory.money_item.count >= player.UseItems[1].value)
        {
            //SlotManager.Instance.FillSlot(player.UseItems[1], 1);
            Inventory.money_item.count -= player.UseItems[1].value;
            Inventory.AddItem(player.UseItems[1], 1);
            SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[33]);
        }
        else
        {
            Inventory.LogMessage("돈이 부족합니다");
        }

    }

    // 업그레이드 버튼
    public void Button_Upgrade_Pick()
    {
        if(Inventory.money_item.count >= player.UpgradeItems[0].value)
        {

            Inventory.money_item.count -= player.UpgradeItems[0].value;
            playerController.pickdamage = pick_damage;
            player.UpgradeItems[0].count++;
            shop_pickLvText.text = "레벨 : " + player.UpgradeItems[0].count;
            player.UpgradeItems[0].value += 10;
            shop_pickUpdateText.text = "-" + player.UpgradeItems[0].value.ToString();

            Inventory.FreshSlot();

            SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[31]);
            Inventory.LogMessage("곡괭이 업그레이드 완료");
        }
        else
        {
            Inventory.LogMessage("돈이 부족합니다");
        }
    }
    public void Button_Upgrade_EyeLight()
    {
        if (Inventory.money_item.count >= player.UpgradeItems[1].value)
        {
            Inventory.money_item.count -= player.UpgradeItems[1].value;
            lightRadius += 0.1f;
            playerlight.GetComponent<Light2D>().pointLightOuterRadius = lightRadius;
            player.UpgradeItems[1].count++;
            shop_lightLvText.text = "레벨 : " + player.UpgradeItems[1].count;
            player.UpgradeItems[1].value += 50;
            shop_lightUpdateText.text = "-" + player.UpgradeItems[1].value.ToString();

            Debug.Log(playerlight.GetComponent<Light2D>().pointLightOuterRadius);
            Inventory.FreshSlot();

            SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[31]);
            Inventory.LogMessage("시야 업그레이드 완료");
        }
        else
        {
            Inventory.LogMessage("돈이 부족합니다");
        }
    }

    public void Button_Create_Drill()
    {
        // 드릴 제작 재료 도면 1, 너트 5, 모터 2
        if(player.Drill_Items[1].count > 0 && player.Drill_Items[2].count > 4 && player.Drill_Items[3].count > 1)
        {
            // 재료 감소
            player.Drill_Items[1].count -= 1;
            player.Drill_Items[2].count -= 5;
            player.Drill_Items[3].count -= 2;

            // 드릴 기능 부여
            isCreateDrill = true;
            //

            createDrill_textobj.SetActive(false);
            upgradeDrill_textobj.SetActive(true);

            player.Drill_Items[0].count++;
            drill_Lv_Text.text = "레벨 : " + player.Drill_Items[0].count;
            Inventory.FreshSlot();

            SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[31]);

            Inventory.LogMessage("드릴을 제작했습니다");
        }
        else
        {
            Inventory.LogMessage("재료가 부족합니다");
        }
    }

    public void Button_Upgrade_Drill()
    {
        // 드릴 강화 재료 도면 1, 너트 2, 모터 1
        if(player.Drill_Items[1].count > 0 && player.Drill_Items[2].count > 1 && player.Drill_Items[3].count > 0)
        {
            // 재료 감소
            player.Drill_Items[1].count -= 1;
            player.Drill_Items[2].count -= 2;
            player.Drill_Items[3].count -= 1;

            // 기능 업그레이드

            //

            player.Drill_Items[0].count++;
            drill_Lv_Text.text = "레벨 : " + player.Drill_Items[0].count;
            Inventory.FreshSlot();

            SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[31]);

            Inventory.LogMessage("드릴을 업그레이드 했습니다");

        }
        else
        {
            Inventory.LogMessage("재료가 부족합니다");
        }
    }

    #region Test
    private void OnGUI()
    {
        GUIStyle bigFontButton = new GUIStyle(GUI.skin.button);
        bigFontButton.fontSize = 30;  // 원하는 글씨 크기

        GUILayout.BeginArea(new Rect(20, 20, 500, 600));
        GUILayout.BeginVertical("box");

        GUILayout.Label("무기 상점", bigFontButton, GUILayout.Height(30));

        //if(GUILayout.Button("무기 강화", bigFontButton, GUILayout.Width(400), GUILayout.Height(50)))
        //{
        //    // 일단 개개인 마다 업그레이드 UI 버튼들이 없기 때문에 동시에 강화를 하는 것으로 함.
        //    List<SlotInfo> allSlots = new();
        //    allSlots.AddRange(quickSlotUI.QuickSlots);
        //    allSlots.AddRange(inventoryUI.InvnetnroySlots);

        //    foreach(var slot in allSlots)
        //    {
        //        if(slot == null || slot._instanceW == null) continue;

        //        var type = slot._instanceW._template.type;

        //        switch(type)
        //        {
        //            case WeaponType.Drill:
        //                slot.UpgradeAndRefresh();

        //                var ui = slot.GetComponentInChildren<SlotInteraction>(true);
        //                if(ui != null)
        //                    ui.Apply(slot._instanceW);

        //                Debug.Log($"[강화] {type} 무기: {slot._instanceW._id} → Lv.{slot._instanceW._level}");
        //                break;

        //            default:
        //                break;
        //        }
        //    }

        //}

        if(GUILayout.Button("텔레포트 생성", bigFontButton, GUILayout.Width(200), GUILayout.Height(50)))
        {
            if(Inventory.money_item.count >= player.UseItems[2].value)
            {
                Inventory.money_item.count -= player.UseItems[2].value;
                //SlotManager.Instance.FillSlot(player.UseItems[2], 3);
                Inventory.AddItem(player.UseItems[2], 3);
                SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[33]);
            }
            else
            {
                Inventory.LogMessage("돈이 부족합니다");
            }
        }

        if(GUILayout.Button("Drill 생성", bigFontButton, GUILayout.Width(200), GUILayout.Height(50)))
        {
            if(Inventory.money_item.count >= player.UseItems[3].value)
            {
                // 드릴 제작 도구를 모두 모았을 때(if문)
                SlotManager.Instance.FillSlot(player.UseItems[3], player.Weapons[1], 1);    // 아이템 추가
                SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[33]);
            }
            else
            {
                Inventory.LogMessage("돈이 부족합니다");
            }
        }

        if(GUILayout.Button("폭탄 생성", bigFontButton, GUILayout.Width(200), GUILayout.Height(50)))
        {
            if(Inventory.money_item.count >= player.UseItems[3].value)
            {
                Inventory.AddItem(player.UseItems[0], 1);
                SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[33]);
            }
            else
            {
                Inventory.LogMessage("돈이 부족합니다");
            }
        }

        if(GUILayout.Button("곡괭이 생성", bigFontButton, GUILayout.Width(200), GUILayout.Height(50)))
        {
            if(Inventory.money_item.count >= player.UseItems[3].value)
            {
                SlotManager.Instance.FillSlot(null, player.Weapons[0], 1);
                SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[33]);
            }
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
    #endregion Test
}

