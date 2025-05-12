using UnityEngine;

public class CamreaFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 posOffset;
    [SerializeField] float smooth;

    Vector3 velocity;

    private void LateUpdate()
    {
        Vector3 newPos = target.position + posOffset;
        // smooth�� ���� Ŭ���� ī�޶� �̵� �ӵ��� ������.
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smooth);  
    }
}
