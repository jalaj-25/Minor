using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [Header("Camera Settings")]
    public Transform defaultCameraPoint;
    public float moveSpeed = 2f;

    private Coroutine moveCoroutine;

    void Awake()
    {
        Instance = this;
    }

    // Move camera to a target position
    public void MoveTo(Transform targetPoint)
    {
        if (targetPoint == null) return;

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(SmoothMove(targetPoint.position, targetPoint.rotation));
    }

    // Return camera to default position
    public void MoveToDefault()
    {
        if (defaultCameraPoint == null) return;

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(SmoothMove(defaultCameraPoint.position, defaultCameraPoint.rotation));
    }

    IEnumerator SmoothMove(Vector3 targetPos, Quaternion targetRot)
    {
        Transform cam = Camera.main.transform;

        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;

            cam.position = Vector3.Lerp(startPos, targetPos, t);
            cam.rotation = Quaternion.Lerp(startRot, targetRot, t);

            yield return null;
        }
    }
}