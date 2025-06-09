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
        // �̹� �ν��Ͻ��� �����ϴ��� Ȯ��
        if (instance == null)
        {
            // �ν��Ͻ��� �������� ������ ���� �ν��Ͻ��� �����ϰ� �������� �ʵ��� ����
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // �ν��Ͻ��� �̹� �����ϸ� �ߺ��Ǵ� �ν��Ͻ��� ����
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // ���� ����� �� ȣ��Ǵ� �̺�Ʈ�� �Լ��� ���
        SceneManager.activeSceneChanged += OnSceneChanged;

    }

    private void OnSceneChanged(Scene currentScene, Scene nextScene)
    {
        
        if (nextScene.name == "csytest")
        {
            // BGM�� ��� ���� ��� ����
            if (BGMSoundPlay.isPlaying)
            {
                BGMSoundPlay.Stop();

                BGMSoundPlay.clip = BGMs[2];
                BGMSoundPlay.Play();
            }
        }
        else if (nextScene.name == "Story")
        {
            // BGM�� ��� ���� ��� ����
            if (BGMSoundPlay.isPlaying)
            {
                BGMSoundPlay.Stop();

                BGMSoundPlay.clip = BGMs[1];
                BGMSoundPlay.Play();
            }
        }
        else if(nextScene.name == "Menu")
        {
            // BGM�� ��� ���� ��� ����
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
