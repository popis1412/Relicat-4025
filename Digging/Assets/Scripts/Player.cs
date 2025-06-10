using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Inventory instance;
    public Inventory Inventory;
    public Collection Collection;
    public Shop Shop;

    public List<Item> items;
    public List<Item> minerals;
    public List<Item> UseItems;
    public List<Item> UpgradeItems;

    // 인벤토리
    [SerializeField] private GameObject Inventory_obj;
    Vector3 Inventory_StartPos;
    Vector3 Inventory_EndPos;
    float currentTime = 0f;
    float moveTime = 1f;
    private bool isInventoryMoving = false;
    private bool isOnInventory = false;


    // 상점
    [SerializeField] private GameObject shopUIPanel; // 상점 UI 패널
    private bool isNearShop = false; // 상점 근처에 있는지 체크

    Vector3 Shop_StartPos;
    Vector3 Shop_EndPos;
    private bool isShopMoving = false;
    private bool isOnShop = false;

    // 박물관
    [SerializeField] private GameObject CollectUIPanel; // 도감 UI 패널
    private bool isNearMuseum = false;
    private bool isInMuseum = false;
    private bool isNearCollection = false;

    Vector3 Collect_StartPos;
    Vector3 Collect_EndPos;
    private bool isCollectMoving = false;
    private bool isOnCollect = false;

    // 진열장
    [SerializeField] private GameObject RelicInfoUIPanel; // 진열장 설명 UI 패널
    [SerializeField] private GameObject RelicInfoImage;
    [SerializeField] private GameObject RelicInfoName;
    [SerializeField] private GameObject RelicInfoText;

    // 체력
    public List<GameObject> li_PlayerHearts = new List<GameObject>(new GameObject[3]);           //플레이어 생명 오브젝트 리스트
    public Dictionary<GameObject, bool> DicPlayerHeart = new Dictionary<GameObject, bool>();

    // 일시정지 탭
    [SerializeField] private GameObject PausePanel;
    private bool isPaused = false;

    [SerializeField] PlayerController player;


    private void Awake()
    {
        // 아이템 초기화
        //for (int i = 0; i < items.Count; i++)
        //{
        //    items[i].count = 0;
        //    items[i].accumulation_count = 0;
        //    items[i].ishaveitem = false;
        //    items[i].isalreadySell = false;

        //}

        //for (int i = 0; i < minerals.Count; i++)
        //{
        //    minerals[i].count = 0;
        //}

        //for(int i = 0; i < UseItems.Count; i++)
        //{
        //    UseItems[i].count = 0;
        //}

        //for(int i = 0; i < UpgradeItems.Count; i++)
        //{
        //    UpgradeItems[i].count = 1;
        //    UpgradeItems[i].value = 10;
        //}

        //플레이어 생명 딕셔너리에 플레이어 생명 리스트에 있는 오브젝트를 저장하고 bool 변수 지정
        DicPlayerHeart.Add(li_PlayerHearts[0], true);
        DicPlayerHeart.Add(li_PlayerHearts[1], true);
        DicPlayerHeart.Add(li_PlayerHearts[2], true);

        player = GetComponent<PlayerController>();

    }

    // Start is called before the first frame update
    private IEnumerator Start()
    {

        // UI 시작/도착 포지션 지정
        Inventory_StartPos = new Vector3(-200f, Screen.height / 2, 0f);
        Inventory_EndPos = new Vector3(0f, Screen.height / 2, 0f);

        Shop_StartPos = new Vector3(2420f, Screen.height / 2, 0f);
        Shop_EndPos = new Vector3(1945f, Screen.height / 2, 0f);

        Collect_StartPos = new Vector3(2420f, Screen.height / 2, 0f);
        Collect_EndPos = new Vector3(1920f, Screen.height / 2, 0f);

        // 한 프레임 대기 (인스턴스 초기화 대기)
        yield return null;

        // Inventory 스크립트 연결
        Inventory = Inventory.Instance ?? FindObjectOfType<Inventory>();
        Shop = Shop.instance ?? FindObjectOfType<Shop>();
        Collection = Collection.instance ?? FindObjectOfType<Collection>();

        // Inventory 오브젝트 연결
        if (Inventory != null)
        {
            Inventory_obj = Inventory.gameObject;
            shopUIPanel = Shop.gameObject;
            CollectUIPanel = Collection.gameObject;
            Debug.Log("Inventory 연결 완료: " + Inventory_obj.name);
        }
        else
        {
            Debug.LogError("Inventory 연결 실패: Inventory.Instance가 null입니다.");
        }

        Inventory_obj.transform.position = new Vector3(-200f, Screen.height / 2, 0f);

        Inventory.badgePanel.gameObject.SetActive(true);
        Inventory.moneyPanel.gameObject.SetActive(true);
        Inventory.healthPanel.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        Interaction_Inventory();
        Interaction_F();

        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene("csytest");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            LostPlayerLife(-1);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            AddPlayerLife(1);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Inventory.AddItem(minerals[0], 1);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Inventory.AddItem(minerals[3], 1);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Inventory.AddItem(items[Random.Range(0,20)], 1);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Inventory.AddItem(items[5], 1);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Inventory.AddItem(items[7], 1);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Collection.collect_sum += 10;
        }
        // 잠시 추가함.
        if(Input.GetKeyDown(KeyCode.Alpha1))
            Inventory.AddItem(UseItems[0], 3);
        if(Input.GetKeyDown(KeyCode.Alpha2))
            Inventory.AddItem(UseItems[1], 3);

        // 일시정지 esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        //세이브
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            SaveSystem.Instance.Save();
        }

        //로드
        if(Input.GetKeyDown(KeyCode.RightBracket))
        {
            SaveSystem.Instance.Load();
        }

    }
    // 게임 재개
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // 게임 일시정지
            PausePanel.SetActive(true); // UI 표시
        }
        else
        {
            Time.timeScale = 1f; // 게임 재개
            PausePanel.SetActive(false); // UI 숨김
        }
    }

    // 설정
    public void SettingButton()
    {
        LoadScene.instance.settingsPanel.SetActive(true);
    }

    // 메인메뉴 이동
    public void GoMainMenu()
    {
        FadeEffect.Instance.OnFade(FadeState.FadeInOut);
        isPaused = false;
        Time.timeScale = 1f; // 게임 재개
        PausePanel.SetActive(false); // UI 숨김
        
        Invoke("InvokeGoMainMenu", 1.5f);
    }
    void InvokeGoMainMenu()
    {
        SceneManager.LoadScene("Menu");
        LoadScene.instance.MainMenu.SetActive(true);
        Inventory.badgePanel.SetActive(false);
        Inventory.moneyPanel.SetActive(false);
        Inventory.healthPanel.SetActive(false);
    }

    // 인벤토리 상호작용
    private void Interaction_Inventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isInventoryMoving && !isOnShop && !isOnCollect)
        {
            currentTime = 0f;
            isInventoryMoving = true;
        }

        // 인벤토리 UI 이동 애니메이션
        if (isInventoryMoving && !isOnInventory)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / moveTime);
            Inventory_obj.transform.position = Vector3.Lerp(Inventory_StartPos, Inventory_EndPos, t);

            if (t >= moveTime)
            {
                isInventoryMoving = false; // 애니메이션 완료
                isOnInventory = true;
            }
        }
        else if (isInventoryMoving && isOnInventory)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / moveTime);
            Inventory_obj.transform.position = Vector3.Lerp(Inventory_EndPos, Inventory_StartPos, t);

            if (t >= moveTime)
            {
                isInventoryMoving = false; // 애니메이션 완료
                isOnInventory = false;
            }
        }
    }

    // F 상호작용
    private void Interaction_F()
    {
        
        // 상점 UI 이동 애니메이션
        if (isShopMoving && !isOnShop)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / moveTime);
            shopUIPanel.transform.position = Vector3.Lerp(Shop_StartPos, Shop_EndPos, t);

            if (t >= moveTime)
            {
                isShopMoving = false; // 애니메이션 완료
                isOnShop = true;
            }
        }
        else if (isShopMoving && isOnShop)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / moveTime);
            shopUIPanel.transform.position = Vector3.Lerp(Shop_EndPos, Shop_StartPos, t);

            if (t >= moveTime)
            {
                isShopMoving = false; // 애니메이션 완료
                isOnShop = false;
            }
        }

        // 상점 상호작용
        if (isNearShop && Input.GetKeyDown(KeyCode.F) && !isInventoryMoving && !isShopMoving)
        {
            currentTime = 0f;

            if (isOnInventory && !isOnShop)
            {
                isShopMoving = true;

            }
            else
            {
                isShopMoving = true;
                isInventoryMoving = true;
            }


        }
        else if (!isNearShop && isOnShop && !isInventoryMoving && !isShopMoving)
        {
            currentTime = 0f;
            isInventoryMoving = true;
            isShopMoving = true;

        }

        // 박물관 상호작용
        if (isNearMuseum && Input.GetKeyDown(KeyCode.F))
        {

            if (!isInMuseum)
            {
                FadeEffect.Instance.OnFade(FadeState.FadeInOut);
                Invoke("InvokeInMuseum", 1.5f);
                SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[16]);

                // 획득한 유물을 도감에 활성화
                for (int i = 0; i < items.Count; i++)
                {
                    Collection.li_isCollect[i] = items[i].ishaveitem;
                    if (Collection.li_isCollect[i] == true)
                    {
                        Collection.slots[i].item = items[i];
                    }
                    if (Collection.li_isRelicOnTable[i] == true)
                    {
                        Collection.Collection_Table[i].GetComponentInChildren<SpriteRenderer>().sprite = items[i].itemImage;
                    }
                }
                

            }
            else if (isInMuseum)
            {
                FadeEffect.Instance.OnFade(FadeState.FadeInOut);
                Invoke("InvokeOutMuseum", 1.5f);
                SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[17]);

            }

        }
        
        // 도감 상호작용

        // 도감 UI 이동 애니메이션
        if (isCollectMoving && !isOnCollect)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / moveTime);
            CollectUIPanel.transform.position = Vector3.Lerp(Collect_StartPos, Collect_EndPos, t);

            if (t >= moveTime)
            {
                isCollectMoving = false; // 애니메이션 완료
                isOnCollect = true;
            }
        }
        else if (isCollectMoving && isOnCollect)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / moveTime);
            CollectUIPanel.transform.position = Vector3.Lerp(Collect_EndPos, Collect_StartPos, t);

            if (t >= moveTime)
            {
                isCollectMoving = false; // 애니메이션 완료
                isOnCollect = false;
            }
        }

        // 도감 상호작용
        if (isNearCollection && Input.GetKeyDown(KeyCode.F) && !isInventoryMoving && !isCollectMoving)
        {
            currentTime = 0f;

            if (isOnInventory && !isOnCollect)
            {
                isCollectMoving = true;

            }
            else
            {
                isCollectMoving = true;
                isInventoryMoving = true;
            }

            
            

        }
        else if (!isNearCollection && isOnCollect && !isInventoryMoving && !isCollectMoving)
        {
            currentTime = 0f;
            isInventoryMoving = true;
            isCollectMoving = true;

        }
    }

    // 박물관 입장/퇴장 페이드 인아웃
    void InvokeInMuseum()
    {
        gameObject.transform.position = new Vector3(50f, 0.5f, 0f);
        isInMuseum = true;

        if (SoundManager.Instance.BGMSoundPlay.isPlaying)
        {
            SoundManager.Instance.BGMSoundPlay.Stop();

            SoundManager.Instance.BGMSoundPlay.clip = SoundManager.Instance.BGMs[3];
            SoundManager.Instance.BGMSoundPlay.Play();
        }
    }
    void InvokeOutMuseum()
    {
        gameObject.transform.position = new Vector3(18f, 0.5f, 0f);
        isInMuseum = false;

        if (SoundManager.Instance.BGMSoundPlay.isPlaying)
        {
            SoundManager.Instance.BGMSoundPlay.Stop();

            SoundManager.Instance.BGMSoundPlay.clip = SoundManager.Instance.BGMs[2];
            SoundManager.Instance.BGMSoundPlay.Play();
        }
    }

    // 플레이어 체력 깎기
    public void LostPlayerLife(int hp)
    {
        
        for (int i = li_PlayerHearts.Count-1; i >= 0; i--)
        {
            if (DicPlayerHeart[li_PlayerHearts[i]] == true)
            {
                DicPlayerHeart[li_PlayerHearts[i]] = false;
                li_PlayerHearts[i].SetActive(false);
                
                if (DicPlayerHeart[li_PlayerHearts[0]] == false)
                {
                    // 플레이어 사망 함수 호출
                    player.Die();
                }
                break;
            }
        }
        
    }

    // 플레이어 체력 증가
    public void AddPlayerLife(int hp)
    {
        
        for (int i = 0; i < li_PlayerHearts.Count; i++)
        {
            int healed = 0;

            if (DicPlayerHeart[li_PlayerHearts[i]] == false)
            {
                DicPlayerHeart[li_PlayerHearts[i]] = true;
                li_PlayerHearts[i].SetActive(true);
                healed++;

                if (healed >= hp) break;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shop"))
        {
            isNearShop = true;
            Debug.Log("상점에 접근했습니다. F 키로 상호작용.");
        }
        if (other.CompareTag("Museum"))
        {
            isNearMuseum = true;
            Debug.Log("박물관에 접근했습니다. F 키로 상호작용.");
        }
        if (other.CompareTag("Collect"))
        {
            isNearCollection = true;
            Debug.Log("도감에 접근했습니다. F 키로 상호작용.");
        }
        if (other.CompareTag("Table"))
        {
            Debug.Log(other.gameObject.name);
            int idx = int.Parse(other.gameObject.name);
            if (Collection.li_isRelicOnTable[idx] == true)
            {
                RelicInfoImage.GetComponent<Image>().sprite = items[idx].itemImage;
                RelicInfoName.GetComponent<TextMeshProUGUI>().text = items[idx].itemName;
                RelicInfoText.GetComponent<TextMeshProUGUI>().text = items[idx].Info;
                RelicInfoUIPanel.SetActive(true);
            }

            
            Debug.Log("진열장에 접근했습니다.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Shop"))
        {
            isNearShop = false;
            Debug.Log("상점에서 벗어났습니다.");
        }
        if (other.CompareTag("Museum"))
        {
            isNearMuseum = false;
            Debug.Log("박물관에서 벗어났습니다.");
        }
        if (other.CompareTag("Collect"))
        {
            isNearCollection = false;
            Debug.Log("도감에서 벗어났습니다.");
        }
        if (other.CompareTag("Table"))
        {
            RelicInfoUIPanel.SetActive(false);
            Debug.Log("진열장에서 벗어났습니다.");
        }
    }

}
