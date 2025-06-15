using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingScene : MonoBehaviour
{
    private int storyNum;
    [SerializeField] private GameObject storyScene;
    [SerializeField] private Sprite[] storyImages;
    private bool isActiveStroy = false;

    // Start is called before the first frame update
    void Start()
    {
        storyNum = 0;
        storyScene.GetComponent<Image>().sprite = storyImages[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActiveStroy && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            isActiveStroy = true;
            storyNum++;
            switch (storyNum)
            {
                case 0:
                    storyScene.GetComponent<Image>().sprite = storyImages[0];
                    break;
                case 1:
                    FadeEffect.Instance.OnFade(FadeState.FadeInOut);
                    Invoke("InvokeLoadStoryIntro01", 1.5f);
                    break;
                case 2:
                    FadeEffect.Instance.OnFade(FadeState.FadeInOut);
                    Invoke("InvokeLoadStoryIntro02", 1.5f);
                    break;
                case 3:
                    FadeEffect.Instance.OnFade(FadeState.FadeInOut);
                    Invoke("InvokeLoadStoryIntro03", 1.5f);
                    break;
                case 4:
                    FadeEffect.Instance.OnFade(FadeState.FadeInOut);
                    Invoke("InvokeLoadStoryIntro04", 1.5f);
                    break;
                case 5:
                    FadeEffect.Instance.OnFade(FadeState.FadeInOut);
                    Invoke("InvokeLoadStoryIntro05", 1.5f);
                    break;
                case 6:
                    FadeEffect.Instance.OnFade(FadeState.FadeInOut);
                    Invoke("InvokeLoadStoryIntro06", 1.5f);
                    break;

                case 7:
                    //Collection.instance.player.Reset_SaveFile_and_GoMenu();
                    //FadeEffect.Instance.OnFade(FadeState.FadeInOut);
                    LoadScene.instance.GoMenu();
                    Invoke("InvokeLoadScene", 1.5f);
                    break;
            }


        }
    }

    void InvokeLoadScene()
    {
        Invoke("invokeMore", 1.5f);
    }
    void InvokeLoadStoryIntro01()
    {
        storyScene.GetComponent<Image>().sprite = storyImages[1];
        Invoke("invokeMore", 1.5f);
    }
    void InvokeLoadStoryIntro02()
    {
        storyScene.GetComponent<Image>().sprite = storyImages[2];
        Invoke("invokeMore", 1.5f);
    }
    void InvokeLoadStoryIntro03()
    {
        storyScene.GetComponent<Image>().sprite = storyImages[3];
        Invoke("invokeMore", 1.5f);
    }
    void InvokeLoadStoryIntro04()
    {
        storyScene.GetComponent<Image>().sprite = storyImages[4];
        Invoke("invokeMore", 1.5f);
    }
    void InvokeLoadStoryIntro05()
    {
        storyScene.GetComponent<Image>().sprite = storyImages[5];
        Invoke("invokeMore", 1.5f);
    }
    void InvokeLoadStoryIntro06()
    {
        storyScene.GetComponent<Image>().sprite = storyImages[6];
        Invoke("invokeMore", 1.5f);
    }
    void invokeMore()
    {
        isActiveStroy = false;
    }
}
