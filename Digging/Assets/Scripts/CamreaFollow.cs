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
        // smooth의 값이 클수록 카메라 이동 속도가 느려짐.
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smooth);  
    }
}
