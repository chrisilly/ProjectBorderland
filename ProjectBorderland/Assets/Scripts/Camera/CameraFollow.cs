using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    float smoothSpeed = 0.125f;
    Vector3 offset = new Vector3(0, 0, -1);

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = desiredPosition;

        transform.LookAt(target);
    }

}
