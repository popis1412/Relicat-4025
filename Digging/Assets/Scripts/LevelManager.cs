using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.ObjectModel;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Collection collection;
    public Shop shop;
    public Player player;

    // �������� ��ǥ ���� ����
    public GameObject stagetargetUI;
    [SerializeField] private TextMeshProUGUI stagetargetNumText;
    private Color originalTargetTextColor;
    public int[] stagetargetNum = new int[2];

    // Ÿ�̸�
    [SerializeField] private TextMeshProUGUI stagetimerText;
    private Color originalTimerTextColor;

    public float totalTime_1 = 900.9f;
    public float totalTime_2 = 1800.9f;
    public float remainingTime;
    public bool isRunning = true;
    public GameObject TimeOutUI;

    public bool isOnEnding = false;

    // �𷡽ð�
    public Animator SandAni_0;
    public Animator SandAni_1;

    // ���̵� UI
    public GameObject GuidePanel;
    public bool isOnGuide = true;
    public GameObject[] Guide_list;
    public int guideView_idx;
    public GameObject left_guideButton;
    public GameObject right_guideButton;

    // ��ư
    public GameObject guide_Button;
    public GameObject pause_Button;

    public bool isClickReset;

    // ����
    //public int stage_Level = 2;

    // ���� ��������
    public GameObject ClearStagePanel_1;
    public GameObject ClearStagePanel_2;
    public bool isStageClear = false;
    public GameObject NextStageButton;
    public GameObject EndingButton;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject); // �ߺ� ����
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
        if (LoadScene.instance.stage_Level == 0)
        {
            remainingTime = totalTime_1;
        }
        else if (LoadScene.instance.stage_Level == 1)
        {
            remainingTime = totalTime_2;
        }
        
        //SaveSystem.Instance.DeleteSaveFile();
        //SaveSystem.Instance.Load();
        originalTargetTextColor = stagetargetNumText.color;
        originalTimerTextColor = stagetimerText.color;

        guideView_idx = 0;
    }
    private void OnDestroy()
    {
        // �̺�Ʈ ���� (�ߺ� ����)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 2) return;

        // ���� �ε�� �� Player �ٽ� ã��
        player = FindObjectOfType<Player>();

        if (player != null)
        {
            Debug.Log("�� ��ȯ �� Player ���� �Ϸ�: " + player.name);
        }
        else
        {
            Debug.Log("�� ��ȯ �� Player�� ã�� ���߽��ϴ�.");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (LoadScene.instance.stage_Level == 0)
        {
            stagetargetNumText.text = collection.collect_sum.ToString() + " / 10";
        }
        else if (LoadScene.instance.stage_Level == 1)
        {
            stagetargetNumText.text = collection.collect_sum.ToString() + " / 20";
        }
        
        if(collection.collect_sum >= stagetargetNum[LoadScene.instance.stage_Level] && isStageClear == false)
        {
            stagetargetNumText.color = Color.green;
            stagetimerText.color = Color.green;

            isRunning = false;
            SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[37]);
            isStageClear = true;

            if (LoadScene.instance.stage_Level == 0)
            {
                ClearStagePanel_1.SetActive(true);
            }
            else if (LoadScene.instance.stage_Level == 1)
            {
                ClearStagePanel_2.SetActive(true);
            }
            
            collection.player.player.input.Disable();
        }
        
        if(stagetargetUI.activeSelf == true && !isStageClear)
        {
            if (!isRunning)
            {
                SandAni_0.speed = 0f;
                SandAni_1.speed = 0f;

                return;
            }

            if (remainingTime > 0f)
            {
                remainingTime -= Time.deltaTime;
                UpdateTimerUI();
                if (remainingTime < 60)
                {
                    stagetimerText.color = Color.red;
                }
                else if (remainingTime < totalTime_1 / 2)
                {
                    stagetimerText.color = Color.yellow;
                }

                SandAni_0.speed = 1f;
                SandAni_1.speed = 1f;

            }
            else
            {
                remainingTime = 0f;
                isRunning = false;
                UpdateTimerUI(); // ������ 00:00 ǥ��
                Debug.Log("Ÿ�̸� ����");

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
    
    public void EndingSequence()
    {
        isOnEnding = true;

        SaveSystem.Instance.DeleteSaveFile();
        LoadScene.instance.isAlreadyWatchStory = false;
        ResetGame();
        Collection.instance.player.Inventory_obj.SetActive(false);
        Collection.instance.player.CollectUIPanel.SetActive(false);
        //Inventory.badgePanel.SetActive(false);
        //Inventory.moneyPanel.SetActive(false);
        //Inventory.healthPanel.SetActive(false);
        stagetargetUI.SetActive(false);
        isOnEnding = false;
        guide_Button.SetActive(false);
        pause_Button.SetActive(false);

        //SaveSystem.Instance.Save();

        LoadScene.instance.GoEnding();

        LoadScene.instance.stage_Level = 0;
        ClearStagePanel_2.SetActive(false);
        EndingButton.SetActive(false);
    }

    public void GoNextStageButton()
    {
        LoadScene.instance.stage_Level = 1;
        if (LoadScene.instance.stage_Level == 0)
        {
            remainingTime = totalTime_1;
        }
        else if (LoadScene.instance.stage_Level == 1)
        {
            remainingTime = totalTime_2;
        }
        stagetargetNumText.color = originalTargetTextColor;
        stagetimerText.color = originalTimerTextColor;

        collection.collect_sum = 0;
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[28]);
        //isStageClear = false;

        ClearStagePanel_1.SetActive(false);
        NextStageButton.SetActive(false);
        collection.player.player.input.Enable();

        LoadScene.instance.GoMain();
        SaveSystem.Instance.Save();
        
    }

    public void ContinueStageButton1()
    {
        collection.player.player.input.Enable();
        ClearStagePanel_1.SetActive(false);
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[28]);
        SaveSystem.Instance.Save();
        NextStageButton.SetActive(true);
    }
    public void ContinueStageButton2()
    {
        collection.player.player.input.Enable();
        ClearStagePanel_2.SetActive(false);
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[28]);
        SaveSystem.Instance.Save();
        EndingButton.SetActive(true);
    }

    public void CloseClearPanel_1()
    {
        LoadScene.instance.stage_Level = 1;
        if (LoadScene.instance.stage_Level == 0)
        {
            remainingTime = totalTime_1;
        }
        else if (LoadScene.instance.stage_Level == 1)
        {
            remainingTime = totalTime_2;
        }
        stagetargetNumText.color = originalTargetTextColor;
        stagetimerText.color = originalTimerTextColor;

        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[28]);
        isStageClear = false;

        ClearStagePanel_1.SetActive(false);
        collection.player.player.input.Enable();

        LoadScene.instance.GoMenu();
        SaveSystem.Instance.Save();

        stagetargetUI.SetActive(false);
        guide_Button.SetActive(false);
        pause_Button.SetActive(false);
        NextStageButton.SetActive(false);

        Collection.instance.player.Inventory_obj.SetActive(false);
        Collection.instance.player.CollectUIPanel.SetActive(false);
    }
    public void CloseClearPanel_2()
    {
        LoadScene.instance.stage_Level = 1;
        if (LoadScene.instance.stage_Level == 0)
        {
            remainingTime = totalTime_1;
        }
        else if (LoadScene.instance.stage_Level == 1)
        {
            remainingTime = totalTime_2;
        }
        stagetargetNumText.color = originalTargetTextColor;
        stagetimerText.color = originalTimerTextColor;

        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[28]);
        isStageClear = false;

        ClearStagePanel_2.SetActive(false);
        collection.player.player.input.Enable();

        LoadScene.instance.GoMenu();
        SaveSystem.Instance.Save();

        stagetargetUI.SetActive(false);
        guide_Button.SetActive(false);
        pause_Button.SetActive(false);

        Collection.instance.player.Inventory_obj.SetActive(false);
        Collection.instance.player.CollectUIPanel.SetActive(false);
    }

    public void Restart_this_Stage_Button()
    {
        SaveSystem.Instance.DeleteSaveFile();
        // ������ �ʱ�ȭ
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

        shop.pick_damage = 6;
        shop.lightRadius = 2f;
        shop.playerlight.GetComponent<Light2D>().pointLightOuterRadius = shop.lightRadius;
        shop.shop_pickLvText.text = "���� : 1";
        shop.shop_lightLvText.text = "���� : 1";
        shop.shop_pickUpdateText.text = "-" + Collection.instance.player.UpgradeItems[0].value.ToString();
        shop.shop_lightUpdateText.text = "-" + Collection.instance.player.UpgradeItems[1].value.ToString();

        if(LoadScene.instance.stage_Level == 1)
        {
            remainingTime = totalTime_1;
        }
        else if(LoadScene.instance.stage_Level == 2)
        {
            remainingTime = totalTime_2;
        }
        
        stagetargetNumText.color = originalTargetTextColor;
        stagetimerText.color = originalTimerTextColor;
        isRunning = false;
        Time.timeScale = 1f;

        //SaveSystem.Instance.Save();
        LoadScene.instance.GoMain();
        TimeOutUI.SetActive(false);
    }
    public void Restart_Go_Menu_Button()
    {
        SaveSystem.Instance.DeleteSaveFile();
        // ������ �ʱ�ȭ
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

        shop.pick_damage = 6;
        shop.lightRadius = 2f;
        shop.playerlight.GetComponent<Light2D>().pointLightOuterRadius = shop.lightRadius;
        shop.shop_pickLvText.text = "���� : 1";
        shop.shop_lightLvText.text = "���� : 1";
        shop.shop_pickUpdateText.text = "-" + Collection.instance.player.UpgradeItems[0].value.ToString();
        shop.shop_lightUpdateText.text = "-" + Collection.instance.player.UpgradeItems[1].value.ToString();

        if (LoadScene.instance.stage_Level == 1)
        {
            remainingTime = totalTime_1;
        }
        else if (LoadScene.instance.stage_Level == 2)
        {
            remainingTime = totalTime_2;
        }
        stagetargetNumText.color = originalTargetTextColor;
        stagetimerText.color = originalTimerTextColor;
        isRunning = false;
        Time.timeScale = 1f;

        //SaveSystem.Instance.Save();
        LoadScene.instance.GoMenu();
        TimeOutUI.SetActive(false);

        stagetargetUI.SetActive(false);
        guide_Button.SetActive(false);
        pause_Button.SetActive(false);

        Collection.instance.player.Inventory_obj.SetActive(false);
    }


    // ���� �ʱ�ȭ
    public void ResetGame()
    {
        // ������ �ʱ�ȭ
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

        shop.pick_damage = 6;
        shop.lightRadius = 2f;
        shop.playerlight.GetComponent<Light2D>().pointLightOuterRadius = shop.lightRadius;
        shop.shop_pickLvText.text = "���� : 1";
        shop.shop_lightLvText.text = "���� : 1";
        shop.shop_pickUpdateText.text = "-" + Collection.instance.player.UpgradeItems[0].value.ToString();
        shop.shop_lightUpdateText.text = "-" + Collection.instance.player.UpgradeItems[1].value.ToString();

        stagetargetNumText.color = originalTargetTextColor;
        stagetimerText.color = originalTimerTextColor;
        isRunning = false;
        remainingTime = totalTime_1;
        
        LoadScene.instance.stage_Level = 0;

        Time.timeScale = 1f;

        //SaveSystem.Instance.Save();
        //LoadScene.instance.GoMenu();
        TimeOutUI.SetActive(false);
    }

    public void Toggle_Pause_Panel()
    {
        collection.player.TogglePause();
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[29]);
    }

    public void Toggle_GuidePanel()
    {
        if (!isOnGuide)
        {
            GuidePanel.SetActive(true);
            isOnGuide = true;
        }
        else if(isOnGuide)
        {
            GuidePanel.SetActive(false);
            isOnGuide = false;
            
        }
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[29]);
    }
    
    public void Button_Left()
    {
        guideView_idx -= 1;
        if (guideView_idx <= 0)
        {
            guideView_idx = 0;
            left_guideButton.SetActive(false);
        }
        right_guideButton.SetActive(true);
        Switch_GuideView();
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[30]);
    }

    
    public void Button_Right()
    {
        guideView_idx += 1;
        if (guideView_idx >= Guide_list.Length - 1)
        {
            guideView_idx = Guide_list.Length - 1;
            right_guideButton.SetActive(false);
        }
        left_guideButton.SetActive(true);
        Switch_GuideView();
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[30]);

    }


    public void Switch_GuideView()
    {
        switch (guideView_idx)
        {
            case 0:
                Guide_list[0].SetActive(true);
                Guide_list[1].SetActive(false);
                Guide_list[2].SetActive(false);
                Guide_list[3].SetActive(false);
                Guide_list[4].SetActive(false);
                Guide_list[5].SetActive(false);
                Guide_list[6].SetActive(false);
                Guide_list[7].SetActive(false);
                Guide_list[8].SetActive(false);
                break;
            case 1:
                Guide_list[0].SetActive(false);
                Guide_list[1].SetActive(true);
                Guide_list[2].SetActive(false);
                Guide_list[3].SetActive(false);
                Guide_list[4].SetActive(false);
                Guide_list[5].SetActive(false);
                Guide_list[6].SetActive(false);
                Guide_list[7].SetActive(false);
                Guide_list[8].SetActive(false);
                break;
            case 2:
                Guide_list[0].SetActive(false);
                Guide_list[1].SetActive(false);
                Guide_list[2].SetActive(true);
                Guide_list[3].SetActive(false);
                Guide_list[4].SetActive(false);
                Guide_list[5].SetActive(false);
                Guide_list[6].SetActive(false);
                Guide_list[7].SetActive(false);
                Guide_list[8].SetActive(false);
                break;
            case 3:
                Guide_list[0].SetActive(false);
                Guide_list[1].SetActive(false);
                Guide_list[2].SetActive(false);
                Guide_list[3].SetActive(true);
                Guide_list[4].SetActive(false);
                Guide_list[5].SetActive(false);
                Guide_list[6].SetActive(false);
                Guide_list[7].SetActive(false);
                Guide_list[8].SetActive(false);
                break;
            case 4:
                Guide_list[0].SetActive(false);
                Guide_list[1].SetActive(false);
                Guide_list[2].SetActive(false);
                Guide_list[3].SetActive(false);
                Guide_list[4].SetActive(true);
                Guide_list[5].SetActive(false);
                Guide_list[6].SetActive(false);
                Guide_list[7].SetActive(false);
                Guide_list[8].SetActive(false);
                break;
            case 5:
                Guide_list[0].SetActive(false);
                Guide_list[1].SetActive(false);
                Guide_list[2].SetActive(false);
                Guide_list[3].SetActive(false);
                Guide_list[4].SetActive(false);
                Guide_list[5].SetActive(true);
                Guide_list[6].SetActive(false);
                Guide_list[7].SetActive(false);
                Guide_list[8].SetActive(false);
                break;
            case 6:
                Guide_list[0].SetActive(false);
                Guide_list[1].SetActive(false);
                Guide_list[2].SetActive(false);
                Guide_list[3].SetActive(false);
                Guide_list[4].SetActive(false);
                Guide_list[5].SetActive(false);
                Guide_list[6].SetActive(true);
                Guide_list[7].SetActive(false);
                Guide_list[8].SetActive(false);
                break;
            case 7:
                Guide_list[0].SetActive(false);
                Guide_list[1].SetActive(false);
                Guide_list[2].SetActive(false);
                Guide_list[3].SetActive(false);
                Guide_list[4].SetActive(false);
                Guide_list[5].SetActive(false);
                Guide_list[6].SetActive(false);
                Guide_list[7].SetActive(true);
                Guide_list[8].SetActive(false);
                break;
            case 8:
                Guide_list[0].SetActive(false);
                Guide_list[1].SetActive(false);
                Guide_list[2].SetActive(false);
                Guide_list[3].SetActive(false);
                Guide_list[4].SetActive(false);
                Guide_list[5].SetActive(false);
                Guide_list[6].SetActive(false);
                Guide_list[7].SetActive(false);
                Guide_list[8].SetActive(true);
                break;

        }
    }

    
}
