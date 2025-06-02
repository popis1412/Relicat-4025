using System;
using UnityEngine;
using UnityEngine.Video;

public class CamreaFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 posOffset;
    [SerializeField] float smooth;

    [SerializeField] private ParallaxController parallaxController;
    Vector3 velocity;

    private MapData.MapSize _mapSize;

    float skySize = 4.5f;

    float camWidth;
    float camHeight;

    private void Awake()
    {
        smooth = 0.2f;
        posOffset = new Vector3(0, 0, -10f);
    }
    private void Start()
    {
        // 맵 정보
        _mapSize = new MapData.MapSizeIsometric(new Vector3(15f, 0f, 0f), 31f, 33f, 1f, 1f);

        // 카메라 화면 절반 크기 계산
        Camera cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
    }

    private void Update()
    {
        Vector3 desiredPos = target.position + posOffset;
        Vector3 targetCameraPos;

        if (target.position.x > 45f)
        {
            // 조건 만족 시 Clamp 무시하고 자유 이동
            targetCameraPos = desiredPos;
        }
        else
        {
            // Clamp 적용된 카메라 위치 계산
            float clampedX = Mathf.Clamp(desiredPos.x, _mapSize.MinX + camWidth, _mapSize.MaxX - camWidth);
            // 카메라는 바닥으로부터 '4.5블럭' 위의 공간이 있고, 현재 카메라가 바닥과 배경의 절반만 보이도록 설정되어 있어, 4.5를 절반으로 나눔.
            float clampedY = Mathf.Clamp(desiredPos.y, _mapSize.MinY + camHeight, skySize / 2f - camHeight / 2f );    
            targetCameraPos = new Vector3(clampedX, clampedY, desiredPos.z);
        }

        // 부드러운 이동
        transform.position = Vector3.SmoothDamp(transform.position, targetCameraPos, ref velocity, smooth);
    }
}
