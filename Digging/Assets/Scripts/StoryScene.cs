using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Spine.Unity;

public class StoryScene : MonoBehaviour
{
    private int storyNum;
    [SerializeField] private GameObject storyScene;
    [SerializeField] private Sprite[] storyImages;
    private bool isActiveStroy = true;

    [SerializeField] private SkeletonAnimation storyAni;
    [SerializeField] private SkeletonDataAsset[] storyDataAssets;

    // Start is called before the first frame update
    void Start()
    {
        storyNum = 0;
        storyAni.skeletonDataAsset = storyDataAssets[0];
        storyAni.Initialize(true);
        storyAni.AnimationState.SetAnimation(0, "scene", false);
        Invoke("invokeMore", 6f);
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
                    storyAni.skeletonDataAsset = storyDataAssets[0];
                    Invoke("invokeMore", 6f);
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
        storyAni.skeletonDataAsset = storyDataAssets[1];
        storyAni.Initialize(true);
        storyAni.AnimationState.SetAnimation(0, "scene", false);
        Invoke("invokeMore", 1.5f);
    }
    void InvokeLoadStoryIntro02()
    {
        storyAni.skeletonDataAsset = storyDataAssets[2];
        storyAni.Initialize(true);
        storyAni.AnimationState.SetAnimation(0, "scene", false);
        Invoke("invokeMore", 5f);
    }

    void invokeMore()
    {
        isActiveStroy = false;
    }
}
