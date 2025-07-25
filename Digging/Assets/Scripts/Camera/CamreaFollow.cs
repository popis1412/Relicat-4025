using System;
using UnityEngine;
using UnityEngine.Video;

public class CamreaFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 posOffset;
    [SerializeField] float smooth;

    Vector3 velocity;

    private MapData.MapSize _mapSize;

    float skySize = 6f;

    float camWidth;
    float camHeight;
    float camHalfHeight;

    private void Awake()
    {
        smooth = 0.2f;
        posOffset = new Vector3(0, 0, -10f);
    }
    private void Start()
    {
        // �� ����
        if(LoadScene.instance.stage_Level == 0)
        {
            _mapSize = new MapData.MapSizeIsometric(new Vector3(15f, 0.5f, 0f), 31f, 44f, 1f, 1f);
        }
        else if(LoadScene.instance.stage_Level == 1)
        {
            _mapSize = new MapData.MapSizeIsometric(new Vector3(15f, 0.5f, 0f), 51f, 84f, 1f, 1f);
        }
        

        // ī�޶� ȭ�� ���� ũ�� ���
        Camera cam = Camera.main;
        camHeight = 2 * cam.orthographicSize;
        camHalfHeight = cam.orthographicSize;
        camWidth = camHalfHeight * cam.aspect;
    }

    private void Update()
    {
        Vector3 desiredPos = target.position + posOffset;
        Vector3 targetCameraPos;

        if(LoadScene.instance.stage_Level == 0)
        {
            if (target.position.x > 45f)
            {
                // ���� ���� �� Clamp �����ϰ� ���� �̵�
                targetCameraPos = desiredPos;
            }
            else
            {
                float MaxY = 0.5f - skySize / 2f;
                // Clamp ����� ī�޶� ��ġ ���
                float clampedX = Mathf.Clamp(desiredPos.x, _mapSize.MinX + camWidth, _mapSize.MaxX - camWidth);
                // ���� ī�޶� �ٴڰ� ����� ���ݸ� ���̵��� �����Ǿ� �־�, �������� ����.
                float clampedY = Mathf.Clamp(desiredPos.y, (_mapSize.MaxY + camHeight) * -1, MaxY + camHeight / 2f);
                targetCameraPos = new Vector3(clampedX, clampedY, desiredPos.z);
            }
            // �ε巯�� �̵�
            transform.position = Vector3.SmoothDamp(transform.position, targetCameraPos, ref velocity, smooth);
        }
        else if(LoadScene.instance.stage_Level == 1)
        {
            if (target.position.x > 55f)
            {
                // ���� ���� �� Clamp �����ϰ� ���� �̵�
                targetCameraPos = desiredPos;
            }
            else
            {
                float MaxY = 0.5f - skySize / 2f;
                // Clamp ����� ī�޶� ��ġ ���
                float clampedX = Mathf.Clamp(desiredPos.x, _mapSize.MinX + camWidth, _mapSize.MaxX - camWidth);
                // ���� ī�޶� �ٴڰ� ����� ���ݸ� ���̵��� �����Ǿ� �־�, �������� ����.
                float clampedY = Mathf.Clamp(desiredPos.y, (_mapSize.MaxY + camHeight) * -1, MaxY + camHeight / 2f);
                targetCameraPos = new Vector3(clampedX, clampedY, desiredPos.z);
            }
            // �ε巯�� �̵�
            transform.position = Vector3.SmoothDamp(transform.position, targetCameraPos, ref velocity, smooth);
        }
        

        
    }
}
