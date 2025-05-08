using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    
    public float SoundVolume
    {
        get => PlayerPrefs.GetFloat("SoundVolume", 0.5f);
        set => PlayerPrefs.SetFloat("SoundVolume", value);
    }

    public void LoadNextSceene()
    {
        if(!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
