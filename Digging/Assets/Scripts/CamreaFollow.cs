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
        // �� ����
        _mapSize = new MapData.MapSizeIsometric(new Vector3(15f, 0f, 0f), 31f, 33f, 1f, 1f);

        // ī�޶� ȭ�� ���� ũ�� ���
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
            // ���� ���� �� Clamp �����ϰ� ���� �̵�
            targetCameraPos = desiredPos;
        }
        else
        {
            // Clamp ����� ī�޶� ��ġ ���
            float clampedX = Mathf.Clamp(desiredPos.x, _mapSize.MinX + camWidth, _mapSize.MaxX - camWidth);
            // ī�޶�� �ٴ����κ��� '4.5��' ���� ������ �ְ�, ���� ī�޶� �ٴڰ� ����� ���ݸ� ���̵��� �����Ǿ� �־�, 4.5�� �������� ����.
            float clampedY = Mathf.Clamp(desiredPos.y, _mapSize.MinY + camHeight, skySize / 2f - camHeight / 2f );    
            targetCameraPos = new Vector3(clampedX, clampedY, desiredPos.z);
        }

        // �ε巯�� �̵�
        transform.position = Vector3.SmoothDamp(transform.position, targetCameraPos, ref velocity, smooth);
    }
}
