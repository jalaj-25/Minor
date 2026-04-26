using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StudentMover : MonoBehaviour
{
    public float speed = 3f;
    public float waitTime = 5f;

    private Vector3 target;
    private bool isMoving = false;

    public Building currentType;

    private enum MoveMode
    {
        StayInside,
        NormalMovement,
        LightScatter,
        FullScatter
    }

    private MoveMode currentMode;

    void Start()
    {
        StartCoroutine(RandomMovement());
    }

    IEnumerator RandomMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            UpdateModeFromDay();   // 🔥 NEW

            HandleMovement();      // 🔥 NEW
        }
    }

    void UpdateModeFromDay()
    {
        int day = DayManager.Instance.currentDay;

        if (day >= 1 && day <= 5)
            currentMode = MoveMode.StayInside;

        else if (day >= 6 && day <= 7)
            currentMode = MoveMode.FullScatter;

        else if (day >= 8 && day <= 12)
            currentMode = MoveMode.LightScatter;

        else
            currentMode = MoveMode.NormalMovement;
    }

    void HandleMovement()
    {
        switch (currentMode)
        {
            case MoveMode.StayInside:
                MoveInsideBuilding();
                break;

            case MoveMode.NormalMovement:
                MoveRandom(); // your original logic
                break;

            case MoveMode.LightScatter:
                Scatter(10f);
                break;

            case MoveMode.FullScatter:
                Scatter(25f);
                break;
        }
    }

    // ✅ YOUR ORIGINAL LOGIC (UNCHANGED)
    void MoveRandom()
    {
        if (CampusManager.Instance == null) return;

        List<BuildingPoint> all = CampusManager.Instance.allPoints;

        List<BuildingPoint> valid = all.FindAll(p =>
            p.buildingType != null &&
            !p.buildingType.isLocked &&
            p.buildingType != currentType
        );

        if (valid.Count == 0)
        {
            Debug.Log("No unlocked buildings to move!");
            return;
        }

        BuildingPoint targetPoint = valid[Random.Range(0, valid.Count)];

        currentType = targetPoint.buildingType;

        MoveTo(targetPoint.movePoint.position);
    }

    // 🔥 NEW: Stay inside building
    void MoveInsideBuilding()
    {
        if (CampusManager.Instance == null) return;

        List<BuildingPoint> all = CampusManager.Instance.allPoints;

        List<BuildingPoint> valid = all.FindAll(p =>
            p.buildingType != null &&
            !p.buildingType.isLocked
        );

        if (valid.Count == 0) return;

        BuildingPoint point = valid[Random.Range(0, valid.Count)];

        MoveTo(point.movePoint.position);
    }

    // 🔥 NEW: Scatter on ground
    void Scatter(float range)
    {
        Vector3 center = Vector3.zero;

        if (CampusManager.Instance != null && CampusManager.Instance.groundCenter != null)
            center = CampusManager.Instance.groundCenter.position;

        float x = Random.Range(-range, range);
        float z = Random.Range(-range, range);

        Vector3 randomPos = center + new Vector3(x, 0, z);

        MoveTo(randomPos);
    }

    public void MoveTo(Vector3 newTarget)
    {
        target = newTarget;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        Vector3 dir = (target - transform.position);
        dir.y = 0;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            isMoving = false;
        }
    }
}