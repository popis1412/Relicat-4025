using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource BGMSoundPlay;
    public AudioSource EffectSoundPlay;

    public AudioClip[] BGMs;
    public AudioClip[] SFXSounds;

    private static SoundManager instance = null;

    private void Awake()
    {
        // 이미 인스턴스가 존재하는지 확인
        if (instance == null)
        {
            // 인스턴스가 존재하지 않으면 현재 인스턴스를 저장하고 삭제되지 않도록 설정
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 인스턴스가 이미 존재하면 중복되는 인스턴스를 삭제
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 씬이 변경될 때 호출되는 이벤트에 함수를 등록
        SceneManager.activeSceneChanged += OnSceneChanged;

    }

    private void OnSceneChanged(Scene currentScene, Scene nextScene)
    {
        
        if (nextScene.name == "csytest")
        {
            // BGM이 재생 중인 경우 멈춤
            if (BGMSoundPlay.isPlaying)
            {
                BGMSoundPlay.Stop();

                BGMSoundPlay.clip = BGMs[2];
                BGMSoundPlay.Play();
            }
        }
        else if (nextScene.name == "Story")
        {
            // BGM이 재생 중인 경우 멈춤
            if (BGMSoundPlay.isPlaying)
            {
                BGMSoundPlay.Stop();

                BGMSoundPlay.clip = BGMs[1];
                BGMSoundPlay.Play();
            }
        }
        else if(nextScene.name == "Menu")
        {
            // BGM이 재생 중인 경우 멈춤
            if (BGMSoundPlay.isPlaying)
            {
                BGMSoundPlay.Stop();

                BGMSoundPlay.clip = BGMs[0];
                BGMSoundPlay.Play();
            }
        }
    }


    public static SoundManager Instance
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
}
