using UnityEngine;
using System.Collections.Generic;

public class GridPlacement : MonoBehaviour
{
    public float gridSize = 0.5f;
    public GameObject currentPrefab;
    public Transform ground;
    public Vector3 placementRotation;

    private bool deleteMode = false;

    // ✅ STORE ALL OBJECTS IN GRID
    public Dictionary<Vector2Int, GameObject> placedObjects = new Dictionary<Vector2Int, GameObject>();

    public RoadManager roadManager; // assign in inspector

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (deleteMode)
            {
                DeleteObject();
            }
            else if (currentPrefab != null)
            {
                PlaceObject();
            }
        }
    }

    public void SetPrefab(GameObject prefab, Vector3 rotation)
    {
        currentPrefab = prefab;
        placementRotation = rotation;
    }

    void PlaceObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 localHit = hit.point - ground.position;

            int x = Mathf.FloorToInt(localHit.x / gridSize);
            int z = Mathf.FloorToInt(localHit.z / gridSize);

            Vector2Int gridPos = new Vector2Int(x, z);

            if (placedObjects.ContainsKey(gridPos))
            {
                Debug.Log("Already occupied!");
                return;
            }

            Vector3 position = new Vector3(
                x * gridSize + gridSize * 0.5f,
                0,
                z * gridSize + gridSize * 0.5f
            );

            position += ground.position;

            Quaternion rotation = Quaternion.Euler(placementRotation);
            GameObject obj = Instantiate(currentPrefab, position, rotation);

            Debug.Log("PLACED OBJECT: " + obj.name);
            Debug.Log("Prefab used: " + currentPrefab.name);

            placedObjects[gridPos] = obj;

            Building b = obj.GetComponent<Building>();
            if (b != null)
            {
                b.gridPosition = gridPos;
            }

            if (roadManager != null)
            {
                roadManager.CheckAllBuildingConnections();
            }

            // ✅ CALL TASK SYSTEM
            if (TaskManager.Instance != null)
            {
                Debug.Log("Calling TaskManager...");
                TaskManager.Instance.RegisterPlacement(obj);
            }
            else
            {
                Debug.LogError("TaskManager Instance is NULL!");
            }

            currentPrefab = null;
        }
    }

    public void StartBuilding(GameObject prefab, Vector3 rotation)
    {
        currentPrefab = prefab;
        placementRotation = rotation;
        deleteMode = false;
    }

    public void DeleteObject()
    {
        Debug.Log("delete obj clicked!!");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.transform == ground)
                return;

            GameObject obj = hit.collider.gameObject;

            // ⭐ TRY GET BUILDING (BEST WAY)
            Building b = obj.GetComponent<Building>();

            if (b != null)
            {
                Vector2Int gridPos = b.gridPosition;

                if (placedObjects.ContainsKey(gridPos))
                {
                    placedObjects.Remove(gridPos);
                }
            }
            else
            {
                // ⭐ FALLBACK (for non-building objects)
                foreach (var pair in placedObjects)
                {
                    if (pair.Value == obj)
                    {
                        Debug.Log("Removing the object");
                        placedObjects.Remove(pair.Key);
                        break;
                    }
                }
            }

            Destroy(obj);

            Debug.Log("🗑 Object Deleted: " + obj.name);

            if (roadManager != null)
            {
                roadManager.CheckAllBuildingConnections();
            }
        }
    }

    public void EnableDeleteMode()
    {
        deleteMode = true;
        currentPrefab = null;
    }

    public void DisableDeleteMode()
    {
        deleteMode = false;
    }
}