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
    public bool isUseStart;

    [SerializeField] private GameObject[] Button_List;

    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private TextMeshProUGUI continueText;

    [SerializeField] private Texture2D cursorTex;


    // ����
    public int stage_Level = 0;

    // ���̵�
    public int difficulty_level = 1;

    [SerializeField] private GameObject difficulty_panel;
    [SerializeField] private Button[] difficulty_btns;
    [SerializeField] private GameObject[] difficulty_info_panels;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject); // �ߺ� ����
        }

        m_MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        m_MusicBGMSlider.onValueChanged.AddListener(SetMusicVolume);
        m_MusicSFXSlider.onValueChanged.AddListener(SetSFXVolume);

        Cursor.SetCursor(cursorTex, new Vector2(30f, 100f), CursorMode.ForceSoftware);
    }

    public static LoadScene Instance
    {
        get
        {
            if(instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    // ���� ����
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

    private void Start()
    {
        //SaveSystem.Instance.DeleteSaveFile();
        SaveSystem.Instance.LoadForLoadScene();
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
        //���̺�
        if(Input.GetKeyDown(KeyCode.LeftBracket))
        {
            SaveSystem.Instance.Save();
        }

        //�ε�
        if(Input.GetKeyDown(KeyCode.RightBracket))
        {
            SaveSystem.Instance.Load();
        }

        // ���̺� ����
        if(Input.GetKeyDown(KeyCode.O))
        {
            SaveSystem.Instance.DeleteSaveFile();
            isAlreadyWatchStory = false;
            stage_Level = 0;
        }
    }

    // ���ӽ���

    public void GameStartButton()
    {
        difficulty_panel.gameObject.SetActive(true);
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[28]);
    }

    public void GoMain()
    {
        FadeEffect.Instance.OnFade(FadeState.FadeInOut);
        if(isAlreadyWatchStory == false)
        {
            isUseStart = true;
            isAlreadyWatchStory = true;
            Invoke("InvokeLoadStory", 1.5f);
        }
        else if(isAlreadyWatchStory == true)
        {
            Invoke("InvokeLoadMain", 1.5f);

        }

        difficulty_panel.SetActive(false);
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[28]);
    }
    void InvokeLoadMain()
    {
        MainMenu.SetActive(false);
        if(stage_Level == 0)
        {
            SceneManager.LoadScene("Main");
        }
        else if(stage_Level == 1)
        {
            SceneManager.LoadScene("Main 1");
        }
        else if(stage_Level == 2)
        {
            SceneManager.LoadScene("Main 2");
        }

        //SaveSystem.Instance.Load();
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
    void InvokeLoadEnding()
    {
        SceneManager.LoadScene("Ending");
    }

    // ����Ÿ��Ʋ��
    public void GoMenu()
    {
        FadeEffect.Instance.OnFade(FadeState.FadeInOut);
        Invoke("InvokeLoadMenu", 1.5f);
    }

    // ���� ���丮
    public void GoEnding()
    {
        FadeEffect.Instance.OnFade(FadeState.FadeInOut);
        Invoke("InvokeLoadEnding", 1.5f);
    }

    //����
    public void OnSettingsButton()
    {
        settingsPanel.SetActive(true);
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[28]);
    }

    public void OnBackButton()
    {
        settingsPanel.SetActive(false);
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[28]);
    }

    // ��������
    public void OnQuitButton()
    {
        Application.Quit();
    }

    // ���콺 ���� �̺�Ʈ
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

    // ���̵�

    public void difficulty_select_button(int idx)
    {
        difficulty_level = idx;
        for(int i = 0; i < difficulty_info_panels.Length; i++)
        {
            difficulty_info_panels[i].SetActive(false);
        }
        difficulty_info_panels[idx].SetActive(true);

        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[28]);
    }
    public void close_difficulty_panel()
    {
        difficulty_panel.SetActive(false);

        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[28]);
    }

    #region Skip!!!
    /*
    private void OnGUI()
    {
        GUIStyle bigFontButton = new GUIStyle(GUI.skin.button);
        bigFontButton.fontSize = 30;  // ���ϴ� �۾� ũ��

        GUILayout.BeginArea(new Rect(10, 10, 300, 300)); // ��ư ��ġ ����
        GUILayout.BeginVertical();

        if(GUILayout.Button("���� ������", bigFontButton, GUILayout.Width(200), GUILayout.Height(50)))
        {
            // ���� ���� buildIndex�� +1 �ؼ� ���� ������ ��ȯ
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextSceneIndex);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
    */
    #endregion
}
