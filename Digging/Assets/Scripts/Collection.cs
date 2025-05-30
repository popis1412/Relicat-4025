using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Collection : MonoBehaviour
{
    public Inventory Inventory;
    public Player player;

    [SerializeField] private GameObject[] Collection_Table;

    public bool[] li_isCollect;
    public bool[] li_isRelicOnTable;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    public Slot[] slots;

    private int collectView_idx;
    [SerializeField] private GameObject[] collectUIList;

    [SerializeField] private GameObject[] mouseOverlap_Panel_List;
    [SerializeField] private Button[] rewardButton_List;
    [SerializeField] private Item[] badge_items;

    public int collect_sum;

    [SerializeField] private GameObject badgeUI_Icon;
    [SerializeField] private TextMeshProUGUI badgeUI_TitleText;


#if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
#endif

    // Start is called before the first frame update
    void Start()
    {
        li_isCollect = new bool[player.items.Count];
        li_isRelicOnTable = new bool[player.items.Count];


        collect_sum = 0;
        collectView_idx = 0;
        Switch_CollectView();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Button_ResistRelic(int itemNum)
    {
        

        if (player.items[itemNum].count > 0)
        {
            li_isRelicOnTable[itemNum] = true;
            player.items[itemNum].accumulation_count += 1;

            UpdateOnTable();

            Collection_Table[itemNum].GetComponentInChildren<SpriteRenderer>().sprite = player.items[itemNum].itemImage;
        }
        Inventory.SellItem(player.items[itemNum]);
    }

    public void UpdateOnTable()
    {
        collect_sum = 0;
        for(int i = 0; i < li_isRelicOnTable.Length; i++)
        {
            if (li_isRelicOnTable[i] == true)
            {
                collect_sum += 1;
            }
        }
        
    }


    public void Button_Left()
    {
        collectView_idx -= 1;
        if (collectView_idx < 0)
        {
            collectView_idx = 1;
        }
        Switch_CollectView();
    }

    // 상점 이동 오른쪽
    public void Button_Right()
    {
        collectView_idx += 1;
        if (collectView_idx > 1)
        {
            collectView_idx = 0;
        }
        Switch_CollectView();
    }


    private void Switch_CollectView()
    {
        switch (collectView_idx)
        {
            case 0:
                collectUIList[0].SetActive(true);
                collectUIList[1].SetActive(false);
                
                break;
            case 1:
                collectUIList[0].SetActive(false);
                collectUIList[1].SetActive(true);
                
                break;
            
        }
    }

    public void MouseOverIn_01()
    {
        mouseOverlap_Panel_List[0].SetActive(true);
    }
    public void MouseOverOut_01()
    {
        mouseOverlap_Panel_List[0].SetActive(false);
    }
    public void MouseOverIn_02()
    {
        mouseOverlap_Panel_List[1].SetActive(true);
    }
    public void MouseOverOut_02()
    {
        mouseOverlap_Panel_List[1].SetActive(false);
    }
    public void MouseOverIn_03()
    {
        mouseOverlap_Panel_List[2].SetActive(true);
    }
    public void MouseOverOut_03()
    {
        mouseOverlap_Panel_List[2].SetActive(false);
    }
    public void MouseOverIn_04()
    {
        mouseOverlap_Panel_List[3].SetActive(true);
    }
    public void MouseOverOut_04()
    {
        mouseOverlap_Panel_List[3].SetActive(false);
    }
    public void MouseOverIn_05()
    {
        mouseOverlap_Panel_List[4].SetActive(true);
    }
    public void MouseOverOut_05()
    {
        mouseOverlap_Panel_List[4].SetActive(false);
    }

    public void Button_GetReward(int idx)
    {
        
        if(idx == 0)
        {
            if(collect_sum >= 10)
            {
                Inventory.ItemLog(badge_items[0], 1);
                badgeUI_Icon.GetComponent<Image>().sprite = badge_items[0].itemImage;
                badgeUI_TitleText.text = "초심자";
                rewardButton_List[idx].GetComponent<Button>().interactable = false;
            }
            
        }
        else if (idx == 1)
        {
            if (collect_sum >= 20)
            {
                Inventory.ItemLog(Inventory.money_item, 1000);
                Inventory.money += 1000;
                Inventory.FreshSlot();
                badgeUI_TitleText.text = "수집가";
                rewardButton_List[idx].GetComponent<Button>().interactable = false;
            }
            
        }
        else if (idx == 2)
        {
            if (collect_sum >= 30)
            {
                Inventory.ItemLog(badge_items[1], 1);
                badgeUI_Icon.GetComponent<Image>().sprite = badge_items[1].itemImage;
                badgeUI_TitleText.text = "전문가";
                rewardButton_List[idx].GetComponent<Button>().interactable = false;
            }
            
        }
        else if (idx == 3)
        {
            if (collect_sum >= 40)
            {
                Inventory.ItemLog(Inventory.money_item, 3000);
                Inventory.money += 3000;
                Inventory.FreshSlot();
                badgeUI_TitleText.text = "큐레이터";
                rewardButton_List[idx].GetComponent<Button>().interactable = false;
            }
        }
        else if (idx == 4)
        {
            if (collect_sum >= 50)
            {
                Inventory.ItemLog(badge_items[2], 1);
                badgeUI_Icon.GetComponent<Image>().sprite = badge_items[2].itemImage;
                badgeUI_TitleText.text = "고고학자";
                rewardButton_List[idx].GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            Debug.Log("조건이 충족되지 않았습니다.");
        }

    }
}
