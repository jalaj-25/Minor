using UnityEngine;

public class NPCMoveToReception : MonoBehaviour
{
    public Transform standPoint;
    public float moveSpeed = 2f;
    public float stopDistance = 0.2f;

    private bool hasReached = false;

    void Update()
    {
        if (hasReached || standPoint == null) return;

        Vector3 dir = standPoint.position - transform.position;
        dir.y = 0f;

        if (dir.magnitude <= stopDistance)
        {
            hasReached = true;
            return;
        }

        transform.position += dir.normalized * moveSpeed * Time.deltaTime;
    }

    // ✅ Used by ReceptionTriggerLogic
    public bool HasReachedPoint()
    {
        return hasReached;
    }

    // ✅ Call when stand point changes
    public void ResetMovement()
    {
        hasReached = false;
    }
}
