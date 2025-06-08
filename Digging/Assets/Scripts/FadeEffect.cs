using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public enum FadeState { FadeIn = 0, FadeOut, FadeInOut, FadeLoop }

public class FadeEffect : MonoBehaviour
{

    private static FadeEffect instance = null;

    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;

    [SerializeField]
    private AnimationCurve fadeCurve; // 페이드 효과가 적용되는 알파 값을 곡선의 값으로 설정

    public Image image; // 페이드 효과에 적용되는 검은 바탕 이미지
    private FadeState fadeState; // 페이드 효과 상태
    public int FadeEffectNum;

    private void Awake()
    {
        image = GetComponent<Image>();

        // Fade In. 배경의 알파값이 1에서 0으로 (화면이 밝아짐)
        //StartCoroutine(Fade(1, 0));
        // Fade Out. 배경의 알파값이 0에서 1로 (화면이 어두워짐)
        //StartCoroutine(Fade(0, 1));
        if (FadeEffectNum == 0)
        {
            OnFade(FadeState.FadeIn);
        }
        if (FadeEffectNum == 1)
        {
            OnFade(FadeState.FadeOut);
        }
        if (FadeEffectNum == 2)
        {
            OnFade(FadeState.FadeInOut);
        }
        if (FadeEffectNum == 3)
        {
            OnFade(FadeState.FadeLoop);
        }
        //OnFade(FadeState.FadeOut);

        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void OnFade(FadeState state)
    {
        fadeState = state;

        switch (fadeState)
        {
            case FadeState.FadeIn:
                StartCoroutine(Fade(1, 0));
                break;
            case FadeState.FadeOut:
                StartCoroutine(Fade(0, 1));
                break;
            case FadeState.FadeInOut:
            case FadeState.FadeLoop:
                StartCoroutine(FadeInOut());
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator FadeInOut()
    {
        while (true)
        {
            //코루틴 내부에서 코루틴 함수를 호출하면 해당 코루틴 함수가 종료되어야 다음 문장 실행
            yield return StartCoroutine(Fade(0, 1));
            yield return new WaitForSeconds(1);
            yield return StartCoroutine(Fade(1, 0));

            if (fadeState == FadeState.FadeInOut)
            {
                break;
            }
        }
    }

    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {

            // fadeTime으로 나누어서 fadeTime 시간 동안 
            // percent 값이 0에서 1로 증가하도록 함
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            // 알파값을 start부터 end까지 fadeTime 시간 동안 변화시킨다
            Color color = image.color;
            //color.a = Mathf.Lerp(start, end, percent);
            color.a = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));
            image.color = color;

            yield return null;
        }
    }

    public static FadeEffect Instance
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
