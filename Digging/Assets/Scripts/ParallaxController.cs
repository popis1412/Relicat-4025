using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

/// <summary>
/// SpriteRenderer에 머터리얼이 적용된 배경 레이어들을 패럴럭스 효과로 움직입니다.
/// 머터리얼 텍스처 오프셋을 변경하여 시각적 이동 효과를 줍니다.
/// </summary>
public class ParallaxController : MonoBehaviour
{
    [Header("패럴럭스 속도 (0: 정지, 0.5: 느림, 1: 카메라와 동일)")]
    [Range(0f, 1f)]
    [SerializeField] private float parallaxSpeed = 0.3f;

    private Transform cam;
    private Vector3 camStartPos;

    private float distance;

    GameObject[] backgrounds;
    Material[] mat;
    float[] backSpeed;

    float farthestBack;

    private void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.transform.position;

        InitialzeBackgrounds();
    }

    // 자식 오브젝트들을 배경으로 초기화하고 SpriteRenderer에서 Material을 가져온다.
    void InitialzeBackgrounds()
    {
        int backCount = transform.childCount;
        backgrounds = new GameObject[backCount];
        mat = new Material[backCount];
        backSpeed = new float[backCount];

        for(int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            SpriteRenderer sr = backgrounds[i].GetComponent<SpriteRenderer>();

            mat[i] = sr.material;
        }

        BackSpeedCalculate(backCount);
    }

    // 배경 별 속도 설정
    void BackSpeedCalculate(int backCount)
    {
        for(int i = 0; i < backgrounds.Length; i++) // find the farthest background
        {
            if ((backgrounds[i].transform.position.z - cam.position.z) > farthestBack)
            {
                farthestBack = backgrounds[i].transform.position.z - cam.position.z;
            }
        }

        for(int i = 0; i < backCount; i++)  // set the speed of backgrounds
        {
            backSpeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / farthestBack;
        }
    }

    private void LateUpdate()
    {
        distance = cam.position.x - camStartPos.x;

        transform.position = new Vector3(cam.position.x, transform.position.y, transform.position.z);


        for (int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;
            mat[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
        }
    }
}
