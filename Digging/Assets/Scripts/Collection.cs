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

    public Item guessItem;


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
            Destroy(this.gameObject); // �ߺ� ����
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
        // �̺�Ʈ ���� (�ߺ� ����)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� �ε�� �� Player �ٽ� ã��
        player = FindObjectOfType<Player>();

        if (player != null)
        {
            Debug.Log("�� ��ȯ �� Player ���� �Ϸ�: " + player.name);
        }
        else
        {
            Debug.LogWarning("�� ��ȯ �� Player�� ã�� ���߽��ϴ�.");
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
                        Debug.LogWarning($"CollectionTable[{i}] ������Ʈ�� ã�� �� �����ϴ�.");
                    }
                }

                Debug.Log("Collection_Table ���� �Ϸ�.");
            }
            else
            {
                Debug.LogError("Museum ������ 'CollectionTable' ������Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("'Museum' ������Ʈ�� ã�� �� �����ϴ�.");
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
            Debug.Log(player.items[itemNum].accumulation_count);
            UpdateOnTable(itemNum);
            collect_sum += 1;
            Inventory.SellItem(player.items[itemNum]);
        }
        
    }

    public void UpdateOnTable(int itemNum)
    {
        
        for(int i = 0; i < li_isRelicOnTable.Length; i++)
        {
            if (li_isRelicOnTable[i] == true)
            {
                Collection_Table[itemNum].GetComponentInChildren<SpriteRenderer>().sprite = player.items[itemNum].itemImage;
                
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

    // ���� �̵� ������
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
                badgeUI_TitleText.text = "�ʽ���";
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
                badgeUI_TitleText.text = "������";
                rewardButton_List[idx].GetComponent<Button>().interactable = false;
            }
            
        }
        else if (idx == 2)
        {
            if (collect_sum >= 30)
            {
                Inventory.ItemLog(badge_items[1], 1);
                badgeUI_Icon.GetComponent<Image>().sprite = badge_items[1].itemImage;
                badgeUI_TitleText.text = "������";
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
                badgeUI_TitleText.text = "ť������";
                rewardButton_List[idx].GetComponent<Button>().interactable = false;
            }
        }
        else if (idx == 4)
        {
            if (collect_sum >= 50)
            {
                Inventory.ItemLog(badge_items[2], 1);
                badgeUI_Icon.GetComponent<Image>().sprite = badge_items[2].itemImage;
                badgeUI_TitleText.text = "�������";
                rewardButton_List[idx].GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            Debug.Log("������ �������� �ʾҽ��ϴ�.");
        }

    }
}
