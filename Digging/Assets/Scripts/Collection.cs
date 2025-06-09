using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif

public class Collection : MonoBehaviour
{
    public static Collection instance;

    public Inventory Inventory;
    public Player player;

    public GameObject[] Collection_Table;

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
            Destroy(this.gameObject); // 중복 방지
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        li_isCollect = new bool[player.items.Count];
        li_isRelicOnTable = new bool[player.items.Count];


        
        collectView_idx = 0;
        Switch_CollectView();
    }

    private void OnDestroy()
    {
        // 이벤트 제거 (중복 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드된 후 Player 다시 찾기
        player = FindObjectOfType<Player>();

        if (player != null)
        {
            Debug.Log("씬 전환 후 Player 연결 완료: " + player.name);
        }
        else
        {
            Debug.LogWarning("씬 전환 후 Player를 찾지 못했습니다.");
        }

        GameObject museum = GameObject.Find("Museum");
        if (museum != null)
        {
            Transform collectionParent = museum.transform.Find("CollectionTable");
            if (collectionParent != null)
            {
                Collection_Table = new GameObject[20];
                for (int i = 0; i < 20; i++)
                {
                    Transform child = collectionParent.Find(i.ToString());
                    if (child != null)
                    {
                        Collection_Table[i] = child.gameObject;
                    }
                    else
                    {
                        Debug.LogWarning($"CollectionTable[{i}] 오브젝트를 찾을 수 없습니다.");
                    }
                }

                Debug.Log("Collection_Table 연결 완료.");
            }
            else
            {
                Debug.LogError("Museum 하위에 'CollectionTable' 오브젝트가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("'Museum' 오브젝트를 찾을 수 없습니다.");
        }

        
            
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

            UpdateOnTable(itemNum);

            
        }
        Inventory.SellItem(player.items[itemNum]);
    }

    public void UpdateOnTable(int itemNum)
    {
        
        for(int i = 0; i < li_isRelicOnTable.Length; i++)
        {
            if (li_isRelicOnTable[i] == true)
            {
                Collection_Table[itemNum].GetComponentInChildren<SpriteRenderer>().sprite = player.items[itemNum].itemImage;
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
