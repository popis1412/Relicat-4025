using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class LoadScene : MonoBehaviour
{
    public static LoadScene instance;

    [SerializeField] private GameObject[] li_arrowImages;

    public GameObject settingsPanel;
    
    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] private Slider m_MusicMasterSlider;
    [SerializeField] private Slider m_MusicBGMSlider;
    [SerializeField] private Slider m_MusicSFXSlider;

    public GameObject MainMenu;

    public bool isAlreadyWatchStory;

    [SerializeField] private GameObject[] Button_List;
    
    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private TextMeshProUGUI continueText;


    private void Awake()
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

        m_MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        m_MusicBGMSlider.onValueChanged.AddListener(SetMusicVolume);
        m_MusicSFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }
    public static LoadScene Instance
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

    // 사운드 조절
    public void SetMasterVolume(float volume)
    {
        m_AudioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        m_AudioMixer.SetFloat("SE", Mathf.Log10(volume) * 20);
    }

    private void Update()
    {
        if(isAlreadyWatchStory == true)
        {
            Button_List[0].GetComponent<Button>().interactable = false;
            Button_List[1].GetComponent<Button>().interactable = true;
            startText.color = new Color(1, 1, 1, 0.3f);
            continueText.color = Color.white;

        }
        else if(isAlreadyWatchStory == false)
        {
            Button_List[0].GetComponent<Button>().interactable = true;
            Button_List[1].GetComponent<Button>().interactable = false;
            startText.color = Color.white;
            continueText.color = new Color(1, 1, 1, 0.3f);
        }
    }

    // 게임시작
    public void GoMain()
    {
        FadeEffect.Instance.OnFade(FadeState.FadeInOut);
        if (isAlreadyWatchStory == false)
        {
            isAlreadyWatchStory = true;
            Invoke("InvokeLoadStory", 1.5f);
        }
        else if(isAlreadyWatchStory == true)
        {
            Invoke("InvokeLoadMain", 1.5f);
        }
        
        
        
    }
    void InvokeLoadMain()
    {
        MainMenu.SetActive(false);
        SceneManager.LoadScene("Main");
        SaveSystem.Instance.Load();
    }
    void InvokeLoadStory()
    {
        MainMenu.SetActive(false);
        SceneManager.LoadScene("Story");
    }
    void InvokeLoadMenu()
    {
        MainMenu.SetActive(true);
        SceneManager.LoadScene("Menu");
    }

    public void GoMenu()
    {
        FadeEffect.Instance.OnFade(FadeState.FadeInOut);
        Invoke("InvokeLoadMenu", 1.5f);
    }

    //설정
    public void OnSettingsButton()
    {
        settingsPanel.SetActive(true);
    }

    public void OnBackButton()
    {
        settingsPanel.SetActive(false);
    }

    // 게임종료
    public void OnQuitButton()
    {
        Application.Quit();
    }

    // 마우스 오버 이벤트
    public void OnPointerEnter(int idx)
    {
        if(Button_List[idx].GetComponent<Button>().interactable == true)
        {
            li_arrowImages[idx].gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(int idx)
    {
        li_arrowImages[idx].gameObject.SetActive(false);
    }
}
