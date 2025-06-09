using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private GameObject[] li_arrowImages;

    public GameObject settingsPanel;
    
    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] private Slider m_MusicMasterSlider;
    [SerializeField] private Slider m_MusicBGMSlider;
    [SerializeField] private Slider m_MusicSFXSlider;

    private void Awake()
    {
        m_MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        m_MusicBGMSlider.onValueChanged.AddListener(SetMusicVolume);
        m_MusicSFXSlider.onValueChanged.AddListener(SetSFXVolume);
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

    // 게임시작
    public void GoMain()
    {
        FadeEffect.Instance.OnFade(FadeState.FadeInOut);
        Invoke("InvokeLoadScene", 1.5f);
    }

    void InvokeLoadScene()
    {
        SceneManager.LoadScene("Story");
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
        li_arrowImages[idx].gameObject.SetActive(true);
    }

    public void OnPointerExit(int idx)
    {
        li_arrowImages[idx].gameObject.SetActive(false);
    }
}
