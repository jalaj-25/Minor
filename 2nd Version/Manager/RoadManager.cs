using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public GridPlacement grid;
    private bool alreadyConnected = false;
    private HashSet<string> completedConnections = new HashSet<string>();
    public void CheckAllBuildingConnections()
    {
        List<Building> buildings = new List<Building>(FindObjectsOfType<Building>());

        Debug.Log("Checking connections...");

        for (int i = 0; i < buildings.Count; i++)
        {
            for (int j = i + 1; j < buildings.Count; j++)
            {
                Building a = buildings[i];
                Building b = buildings[j];

                if (a.blockType == b.blockType) continue;

                Debug.Log($"Checking {a.blockType} → {b.blockType}");

                string connectionKey = a.blockType + "-" + b.blockType;
                string reverseKey = b.blockType + "-" + a.blockType;

                if (completedConnections.Contains(connectionKey) || completedConnections.Contains(reverseKey))
                    continue;

                if (IsConnected(a.gridPosition, b.gridPosition))
                {
                    Debug.Log($"✅ Connected {a.blockType} ↔ {b.blockType}");

                    GameManager.Instance.ChangeReputation(10);

                    completedConnections.Add(connectionKey);
                }
            }
        }
    }

    bool IsConnected(Vector2Int start, Vector2Int end)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        Vector2Int[] directions = {
        Vector2Int.up, Vector2Int.down,
        Vector2Int.left, Vector2Int.right
    };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            Debug.Log("Visiting: " + current); // ✅ FIXED POSITION

            if (current == end)
                return true;

            foreach (var dir in directions)
            {
                Vector2Int next = current + dir;

                if (visited.Contains(next)) continue;

                if (grid.placedObjects.ContainsKey(next))
                {
                    GameObject obj = grid.placedObjects[next];

                    if (obj.CompareTag("Road") || obj.CompareTag("Building"))
                    {
                        queue.Enqueue(next);
                        visited.Add(next);
                    }
                }
            }
        }

        return false;
    }
}