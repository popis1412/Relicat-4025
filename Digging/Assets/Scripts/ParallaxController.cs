using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

/// <summary>
/// SpriteRenderer�� ���͸����� ����� ��� ���̾���� �з����� ȿ���� �����Դϴ�.
/// ���͸��� �ؽ�ó �������� �����Ͽ� �ð��� �̵� ȿ���� �ݴϴ�.
/// </summary>
public class ParallaxController : MonoBehaviour
{
    [Header("�з����� �ӵ� (0: ����, 0.5: ����, 1: ī�޶�� ����)")]
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

    // �ڽ� ������Ʈ���� ������� �ʱ�ȭ�ϰ� SpriteRenderer���� Material�� �����´�.
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

    // ��� �� �ӵ� ����
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
