using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryScene : MonoBehaviour
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
                    Invoke("InvokeLoadScene", 1.5f);
                    break;
            }


        }
    }

    void InvokeLoadScene()
    {
        Invoke("invokeMore", 1.5f);
        SceneManager.LoadScene(2);
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

    void invokeMore()
    {
        isActiveStroy = false;
    }
}
