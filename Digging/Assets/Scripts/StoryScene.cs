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

    // Start is called before the first frame update
    void Start()
    {
        storyNum = 0;
        storyScene.GetComponent<Image>().sprite = storyImages[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            storyNum++;
            switch (storyNum)
            {
                case 0:
                    storyScene.GetComponent<Image>().sprite = storyImages[0];
                    break;

                case 1:
                    storyScene.GetComponent<Image>().sprite = storyImages[1]; 
                    break;

                case 2:
                    storyScene.GetComponent<Image>().sprite = storyImages[2];
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
        SceneManager.LoadScene(2);
    }
}
