using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.ObjectModel;
using UnityEngine.Rendering.Universal;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Collection collection;
    public Shop shop;

    public GameObject stagetargetUI;
    public GameObject TimeOutUI;
    [SerializeField] private TextMeshProUGUI stagetargetNumText;
    private Color originalTextColor;
    [SerializeField] private TextMeshProUGUI stagetimerText;

    private float totalTime = 300f;
    public float remainingTime;
    public bool isRunning = true;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject); // 중복 방지
        }

    }
    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        SaveSystem.Instance.DeleteSaveFile();
        SaveSystem.Instance.Load();
        remainingTime = totalTime;
        originalTextColor = stagetargetNumText.color;
    }

    // Update is called once per frame
    void Update()
    {
        stagetargetNumText.text = collection.collect_sum.ToString() + " / 10";
        if(collection.collect_sum >= 10)
        {
            stagetargetNumText.color = Color.green;
            stagetimerText.color = Color.green;

            isRunning = false;
        }
        else
        {
            stagetargetNumText.color = originalTextColor;
        }
        if(stagetargetUI.activeSelf == true)
        {
            if (!isRunning) return;

            if (remainingTime > 0f)
            {
                remainingTime -= Time.deltaTime;
                UpdateTimerUI();
                if (remainingTime < 60)
                {
                    stagetimerText.color = Color.red;
                }
                else if (remainingTime < totalTime / 2)
                {
                    stagetimerText.color = Color.yellow;
                }

            }
            else
            {
                remainingTime = 0f;
                isRunning = false;
                UpdateTimerUI(); // 마지막 00:00 표시
                Debug.Log("타이머 종료");

                TimeOutEvent();
            }
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        stagetimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimeOutEvent()
    {
        if(remainingTime <= 0)
        {
            TimeOutUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Restart_this_Stage_Button()
    {
        // 아이템 초기화
        for (int i = 0; i < collection.player.items.Count; i++)
        {
            collection.player.items[i].count = 0;
            collection.player.items[i].accumulation_count = 0;
            collection.player.items[i].ishaveitem = false;
            collection.player.items[i].isalreadySell = false;
        }
        for (int i = 0; i < collection.player.minerals.Count; i++)
        {
            collection.player.minerals[i].count = 0;
        }

        for (int i = 0; i < collection.player.UseItems.Count; i++)
        {
            collection.player.UseItems[i].count = 0;
        }

        
        collection.player.UpgradeItems[0].count = 1;
        collection.player.UpgradeItems[0].value = 10;
        collection.player.UpgradeItems[1].count = 1;
        collection.player.UpgradeItems[1].value = 50;

        collection.Inventory.money_item.count = 0;
        collection.Inventory.ClearItem();

        collection.li_isCollect = new bool[collection.player.items.Count];
        collection.li_isRelicOnTable = new bool[collection.player.items.Count];
        for (int i = 0; i < collection.player.items.Count; i++)
        {
            collection.slots[i].item = collection.guessItem;
            collection.Collection_Table[i].GetComponentInChildren<SpriteRenderer>().sprite = null;
        }
        collection.collect_sum = 0;

        shop.playerController.pickdamage = 6;
        shop.lightRadius = 1.5f;
        shop.playerlight.GetComponent<Light2D>().pointLightOuterRadius = shop.lightRadius;
        shop.shop_pickLvText.text = "레벨 : 1";
        shop.shop_lightLvText.text = "레벨 : 1";

        remainingTime = totalTime;

        Time.timeScale = 1f;

        SaveSystem.Instance.Save();
        LoadScene.instance.GoMain();
        TimeOutUI.SetActive(false);
    }
    public void Restart_Go_Menu_Button()
    {
        // 아이템 초기화
        for (int i = 0; i < collection.player.items.Count; i++)
        {
            collection.player.items[i].count = 0;
            collection.player.items[i].accumulation_count = 0;
            collection.player.items[i].ishaveitem = false;
            collection.player.items[i].isalreadySell = false;
        }
        for (int i = 0; i < collection.player.minerals.Count; i++)
        {
            collection.player.minerals[i].count = 0;
        }

        for (int i = 0; i < collection.player.UseItems.Count; i++)
        {
            collection.player.UseItems[i].count = 0;
        }


        collection.player.UpgradeItems[0].count = 1;
        collection.player.UpgradeItems[0].value = 10;
        collection.player.UpgradeItems[1].count = 1;
        collection.player.UpgradeItems[1].value = 50;

        collection.Inventory.money_item.count = 0;
        collection.Inventory.ClearItem();

        collection.li_isCollect = new bool[collection.player.items.Count];
        collection.li_isRelicOnTable = new bool[collection.player.items.Count];
        for (int i = 0; i < collection.player.items.Count; i++)
        {
            collection.slots[i].item = collection.guessItem;
            collection.Collection_Table[i].GetComponentInChildren<SpriteRenderer>().sprite = null;
        }
        collection.collect_sum = 0;

        shop.playerController.pickdamage = 6;
        shop.lightRadius = 1.5f;
        shop.playerlight.GetComponent<Light2D>().pointLightOuterRadius = shop.lightRadius;
        shop.shop_pickLvText.text = "레벨 : 1";
        shop.shop_lightLvText.text = "레벨 : 1";

        remainingTime = totalTime;

        Time.timeScale = 1f;

        SaveSystem.Instance.Save();
        LoadScene.instance.GoMenu();
        TimeOutUI.SetActive(false);
    }
}
