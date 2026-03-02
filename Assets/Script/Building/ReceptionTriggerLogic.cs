using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReceptionTriggerLogic : MonoBehaviour
{
    [Header("NPCs")]
    public List<Transform> npcPool;

    [Header("Stand Points")]
    public Transform[] initialStandPoints;
    public Transform[] extraStandPoints;

    [Header("Money")]
    public PlayerMoney playerMoney;
    public int rewardAmount = 100;

    [Header("Service Settings")]
    public float serviceCooldown = 1.5f;

    private Queue<Transform> npcQueue = new Queue<Transform>();
    private List<Transform> activeStandPoints = new List<Transform>();

    private bool canServe = true;
    private bool npcsWaiting = false;

    void Start()
    {
        activeStandPoints.AddRange(initialStandPoints);

        foreach (Transform p in extraStandPoints)
            p.gameObject.SetActive(false);

        foreach (Transform npc in npcPool)
            npcQueue.Enqueue(npc);

        AssignStandPointsToQueue();
    }

    void Update()
    {
        CheckNPCStatus();
    }

    void CheckNPCStatus()
    {
        bool allReached = true;
        int index = 0;

        foreach (Transform npc in npcQueue)
        {
            if (index >= activeStandPoints.Count) break;

            NPCMoveToReception mover = npc.GetComponent<NPCMoveToReception>();
            if (mover == null || !mover.HasReachedPoint())
            {
                allReached = false;
                break;
            }

            index++;
        }

        npcsWaiting = allReached && npcQueue.Count > 0;
    }


    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!npcsWaiting) return;
        if (!canServe) return;
        if (npcQueue.Count == 0) return;

        Transform frontNPC = npcQueue.Peek();
        NPCMoveToReception mover = frontNPC.GetComponent<NPCMoveToReception>();

        if (mover == null || !mover.HasReachedPoint())
            return;

        StartCoroutine(ServeNPC());
    }

    //private IEnumerator ServeNPC()
    //{
    //    canServe = false;

    //    Transform frontNPC = npcQueue.Dequeue();
    //    playerMoney.AddMoney(rewardAmount);
    //    frontNPC.gameObject.SetActive(false);

    //    ReassignStandPoints();

    //    yield return new WaitForSeconds(serviceCooldown);

    //    canServe = true;
    //}

    void AssignStandPointsToQueue()
    {
        int index = 0;

        foreach (Transform npc in npcQueue)
        {
            NPCMoveToReception mover = npc.GetComponent<NPCMoveToReception>();

            if (index < activeStandPoints.Count)
            {
                mover.standPoint = activeStandPoints[index];
                mover.ResetMovement();
            }
            else
            {
                mover.standPoint = null;
            }

            index++;
        }
    }
    private IEnumerator ServeNPC()
    {
        canServe = false;

        Transform frontNPC = npcQueue.Dequeue();

        playerMoney.AddMoney(rewardAmount);
        frontNPC.gameObject.SetActive(false);

        ReassignStandPoints();

        yield return new WaitForSeconds(serviceCooldown);

        canServe = true;
    }
    public bool CanServe()
    {
        if (!npcsWaiting) return false;
        if (!canServe) return false;
        if (npcQueue.Count == 0) return false;

        Transform frontNPC = npcQueue.Peek();
        NPCMoveToReception mover = frontNPC.GetComponent<NPCMoveToReception>();

        if (mover == null || !mover.HasReachedPoint())
            return false;

        return true;
    }

    public void AutoServeNPC()
    {
        if (!CanServe()) return;

        StartCoroutine(ServeNPC());
    }

    void ReassignStandPoints()
    {
        AssignStandPointsToQueue();
    }
}