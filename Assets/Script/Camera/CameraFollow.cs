using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;     // Player
    public Vector3 offset;        // Camera offset
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.position = smoothPosition;
        transform.LookAt(target);
    }
}
