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
    private AnimationCurve fadeCurve; // ���̵� ȿ���� ����Ǵ� ���� ���� ��� ������ ����

    public Image image; // ���̵� ȿ���� ����Ǵ� ���� ���� �̹���
    private FadeState fadeState; // ���̵� ȿ�� ����
    public int FadeEffectNum;

    private void Awake()
    {
        image = GetComponent<Image>();

        // Fade In. ����� ���İ��� 1���� 0���� (ȭ���� �����)
        //StartCoroutine(Fade(1, 0));
        // Fade Out. ����� ���İ��� 0���� 1�� (ȭ���� ��ο���)
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
            //�ڷ�ƾ ���ο��� �ڷ�ƾ �Լ��� ȣ���ϸ� �ش� �ڷ�ƾ �Լ��� ����Ǿ�� ���� ���� ����
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

            // fadeTime���� ����� fadeTime �ð� ���� 
            // percent ���� 0���� 1�� �����ϵ��� ��
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            // ���İ��� start���� end���� fadeTime �ð� ���� ��ȭ��Ų��
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
