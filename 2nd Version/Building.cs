using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public string buildingName; 
    public string blockType; // Set in Inspector: Block1, Block2, Block3
    //public 
    [Header("Assigned People")]
    public int assignedStudents;
    public int assignedTeachers;

    [Header("Capacity")]
    public int maxStudents = 10;
    public int maxTeachers = 2;
    public int maxLevel = 5;

    [Header("Students")]
    public List<GameObject> studentPrefabs;
    public List<GameObject> teacehrPrefabs;
    private List<GameObject> activeStudents = new List<GameObject>();
    private List<GameObject> activeTeachers = new List<GameObject>();

    [Header("Building & Upgrade")]
    public Transform pointA;
    public Transform pointB;

    public int level = 1;

    public int upgradeCost = 500;
    public int upgradeCostIncrease = 50;

    public int teachersIncrease = 1;

    [Header("Teacher Student Ratio")]
    public int studentsPerTeacher = 25;
    public int qualityWarningLimit = 25;
    public int redAlertLimit = 40;
    public int absoluteLimit = 45;
    public int teacherHiringCost = 20;

    [Header("Student Income Upgrade")]
    public int studentIncomeLevel = 1;
    public int maxIncomeLevel = 5;

    public int studentsIncrease = 10;
    public int studentIncomeUpgradeCost = 200;
    public int studentIncomeUpgradeCostIncrease = 150;

    public int baseIncome = 1;
    public int incomeIncrease = 2;

    [Header("Income System")]
    public int moneyPerStudent = 1;      // how much each student earns
    public float incomeInterval = 10f;   // time between income generation

    public int baseStudentCapacity = 20;
    public int capacityIncrease = 10;

    public int baseUpgradeCost = 500;

    public int baseTeachers = 1;

    public Vector2Int gridPosition;

    [Header("UI")]
    public GameObject interactionPanel;
    public Button upgradeStudentIncomeButton;
    public TextMeshProUGUI studentText;
    public TextMeshProUGUI teacherText;
    public TextMeshProUGUI levelText;

    [Header("Buttons")]
    public Button assignStudentButton;
    public Button relocateStudentButton;
    public Button assignTeacherButton;
    public Button relocateTeacherButton;
    public Button unlockButton;

    [Header("Hover UI")]
    public GameObject buildingNameLabel;

    [Header("Camera")]
    public Transform cameraPoint;
    private Vector3 previousCamPos;
    private Quaternion previousCamRot;

    [Header("Unlock Camera")]
    public Transform unlockCameraPoint;   // assign in Inspector
    public float cameraStayDuration = 2f;

    [Header("Building Lock")]
    public bool isLocked = true;
    public int unlockCost = 1000;
    public GameObject buildingVisual; // assign your building prefab/mesh here
    //public Renderer buildingRenderer;

    private static GameObject currentPanel;

    void Start()
    {
        Debug.Log(buildingName + " position: " + gridPosition);

        maxStudents = baseStudentCapacity + (level - 1) * capacityIncrease;
        maxTeachers = baseTeachers + (level - 1);

        moneyPerStudent = baseIncome + (studentIncomeLevel - 1) * incomeIncrease;

        // ⭐ NEW LOGIC
        if (buildingVisual != null)
        {
            buildingVisual.SetActive(!isLocked);
        }

        if (unlockButton != null)
        {
            unlockButton.gameObject.SetActive(isLocked);
        }
        buildingVisual.SetActive(false);
    }

    void OnMouseEnter()
    {
        if (buildingNameLabel != null)
            buildingNameLabel.SetActive(true);
    }

    void OnMouseExit()
    {
        if (buildingNameLabel != null)
            buildingNameLabel.SetActive(false);
    }

    void OnMouseDown()
    {

        if (currentPanel != null)
            currentPanel.SetActive(false);

        interactionPanel.SetActive(true);
        currentPanel = interactionPanel;

        RefreshUI();

        // Move camera to this building
        if (CameraController.Instance != null)
        {
            CameraController.Instance.MoveTo(cameraPoint);
        }
    }

    public void RefreshUI()
    {
        studentText.text = "Students : " + assignedStudents + "/" + maxStudents;
        teacherText.text = "Teachers : " + assignedTeachers + "/" + maxTeachers;
        levelText.text = "Level : " + level;

        UpdateButtons();
    }

    void UpdateButtons()
    {
        int allowedStudents = assignedTeachers * studentsPerTeacher;
        if (upgradeStudentIncomeButton != null)
        {
            upgradeStudentIncomeButton.interactable =
                GameManager.Instance.money >= studentIncomeUpgradeCost &&
                studentIncomeLevel < maxIncomeLevel;
        }

        assignStudentButton.interactable =
            GameManager.Instance.freeStudents >= 5 &&
            assignedStudents < maxStudents &&
            assignedTeachers > 0 &&
            assignedStudents < allowedStudents;

        relocateStudentButton.interactable =
            assignedStudents >= 5;

        assignTeacherButton.interactable =
            GameManager.Instance.freeTeachers > 0 &&
            assignedTeachers < maxTeachers;

        relocateTeacherButton.interactable =
            assignedTeachers > 0;

        if (unlockButton != null)
        {
            unlockButton.interactable =
                GameManager.Instance.money >= upgradeCost &&
                level < maxLevel;
        }
    }

    public void AssignStudent()
    {
        int amount = 5;
        if (assignedTeachers <= 0)
        {
            Debug.Log("Assign a teacher before assigning students.");
            return;
        }

        int allowedStudents = assignedTeachers * studentsPerTeacher;

        if (assignedStudents + amount > allowedStudents)
        {
            AudioManager.Instance.PlayFullCapacity();
            Debug.Log("Too many students for available teachers.");
            return;
        }

        if (GameManager.Instance.freeStudents >= amount &&
            assignedStudents + amount <= maxStudents)
        {
            AudioManager.Instance.PlayMoneySound();
            assignedStudents += amount;
            GameManager.Instance.freeStudents -= amount;

            int earnedMoney = amount * moneyPerStudent;
            GameManager.Instance.AddMoney(earnedMoney);
            GameManager.Instance.ChangeHappiness(1);

            for (int i = 0; i < amount; i++)
            {
                // 🔥 Pick random student prefab
                GameObject randomPrefab = studentPrefabs[Random.Range(0, studentPrefabs.Count)];

                GameObject student = Instantiate(randomPrefab, pointA.position, Quaternion.identity);

                StudentMover mover = student.GetComponent<StudentMover>();
                Debug.Log("Prefabs count: " + studentPrefabs.Count);
                Debug.Log("Picked prefab: " + randomPrefab); 
                Debug.Log("PointA: " + pointA);
                Debug.Log("PointB: " + pointB);
                // Optional: add small random spread
                if (mover == null)
                {
                    mover = student.AddComponent<StudentMover>(); // auto fix
                }
                mover.MoveTo(pointB.position);

                activeStudents.Add(student);
            }

            CheckTeachingQuality();
            RefreshUI();
            GameManager.Instance.UpdateTopUI();
        }
    }

    public void RelocateStudent()
    {
        int amount = 5;

        if (activeStudents.Count < amount)
        {
            Debug.Log("Not enough students to relocate.");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject student = activeStudents[0];
            activeStudents.RemoveAt(0);

            StudentMover mover = student.GetComponent<StudentMover>();
            if (mover != null)
            {
                mover.MoveTo(pointA.position);
            }
        }

        assignedStudents -= amount;
        GameManager.Instance.freeStudents += amount;

        RefreshUI();
        GameManager.Instance.UpdateTopUI();
    }

    public void AssignTeacher()
    {
        int amount = 1;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayTeacherLeaveSound(); // or create separate assign sound
        }
        if (GameManager.Instance.freeTeachers >= amount &&
            assignedTeachers + amount <= maxTeachers)
        {
            int totalCost = amount * teacherHiringCost;

            // ❗ Check money first
            if (GameManager.Instance.money < totalCost)
            {
                AudioManager.Instance.PlayFullCapacity();
                Debug.Log("Not enough money to hire teacher");
                return;
            }

            // 🔥 Deduct money
            GameManager.Instance.SpendMoney(totalCost);

            assignedTeachers += amount;
            GameManager.Instance.freeTeachers -= amount;

            // Spawn teacher (your existing code)
            for (int i = 0; i < amount; i++)
            {
                GameObject randomPrefab = studentPrefabs[Random.Range(0, studentPrefabs.Count)];

                GameObject teacher = Instantiate(randomPrefab, pointA.position, Quaternion.identity);

                StudentMover mover = teacher.GetComponent<StudentMover>();
                if (mover == null)
                    mover = teacher.AddComponent<StudentMover>();

                mover.MoveTo(pointB.position);

                activeTeachers.Add(teacher);
            }

            RefreshUI();
            GameManager.Instance.UpdateTopUI();
        }
    }

    public void RelocateTeacher()
    {
        int amount = 1;

        if (activeTeachers.Count < amount)
        {
            Debug.Log("No teachers to relocate.");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject teacher = activeTeachers[0];
            activeTeachers.RemoveAt(0);

            StudentMover mover = teacher.GetComponent<StudentMover>();
            if (mover != null)
            {
                mover.MoveTo(pointA.position);
            }
        }

        assignedTeachers -= amount;
        GameManager.Instance.freeTeachers += amount;

        RefreshUI();
        GameManager.Instance.UpdateTopUI();
    }

    public void UpgradeBuilding()
    {
        if (level >= maxLevel)
        {
            AudioManager.Instance.PlayFullCapacity();
            Debug.Log("Max level reached. Build new building.");
            return;
        }

        if (GameManager.Instance.money < upgradeCost)
        {
            Debug.Log("Not enough money to upgrade building. Required: $" + upgradeCost);
            return;
        }

        // Show cost before deduction
        Debug.Log("Upgrading " + buildingName + " | Cost: $" + upgradeCost);
        AudioManager.Instance.UpdateBuildingClick();
        GameManager.Instance.SpendMoney(upgradeCost);

        level++;

        maxStudents = baseStudentCapacity + (level - 1) * capacityIncrease;
        maxTeachers = baseTeachers + (level - 1);

        upgradeCost = baseUpgradeCost + (level - 1) * upgradeCostIncrease;

        Debug.Log("Upgrade complete. New Level: " + level + " | Next Upgrade Cost: $" + upgradeCost);

        GameManager.Instance.ChangeReputation(3);
        GameManager.Instance.ChangeHappiness(2);
        RefreshUI();
        GameManager.Instance.UpdateTopUI();
    }

    public void UpgradeStudentIncome()
    {
        Debug.Log("Students income would be increasedd!!!!!!!");
        if (studentIncomeLevel >= maxIncomeLevel)
        {
            Debug.Log("Student income already at max level.");
            return;
        }

        if (GameManager.Instance.money < studentIncomeUpgradeCost)
        {
            Debug.Log("Not enough money. Required: $" + studentIncomeUpgradeCost);
            return;
        }

        Debug.Log("Upgrading student income | Cost: $" + studentIncomeUpgradeCost);

        GameManager.Instance.SpendMoney(studentIncomeUpgradeCost);

        studentIncomeLevel++;

        // Calculate new income
        moneyPerStudent = baseIncome + (studentIncomeLevel - 1) * incomeIncrease;

        studentIncomeUpgradeCost += studentIncomeUpgradeCostIncrease;

        Debug.Log("Student income upgraded. Level: "
            + studentIncomeLevel
            + " | Income per student: $"
            + moneyPerStudent);

        GameManager.Instance.ChangeReputation(1);
        RefreshUI();
        GameManager.Instance.UpdateTopUI();
    }

    void CheckTeachingQuality()
    {
        if (assignedTeachers == 0)
            return;

        int allowedStudents = assignedTeachers * studentsPerTeacher;

        float ratio = (float)assignedStudents / allowedStudents;

        if (ratio >= 0.9f)
        {
            Debug.Log("RED ALERT: Too many students per teacher!");
            GameManager.Instance.ChangeHappiness(-5);
            GameManager.Instance.ChangeReputation(-3);
        }
        else if (ratio >= 0.8f)
        {
            Debug.Log("Assign more teacher to maintain high quality of teaching.");
            GameManager.Instance.ChangeHappiness(-2);
        }
    }

    public int GenerateDailyIncome()
    {
        if (isLocked)
            return 0;

        int income = assignedStudents * moneyPerStudent;

        int reputationBonus = GameManager.Instance.reputation / 10;

        income += reputationBonus;

        return income;
    }

    public void TryUnlockBuilding()
    {
        if (!isLocked)
            return;

        if (GameManager.Instance.money < unlockCost)
        {
            Debug.Log("Not enough money to unlock.");
            return;
        }

        GameManager.Instance.SpendMoney(unlockCost);

        isLocked = false;

        // ⭐ SHOW BUILDING
        if (buildingVisual != null)
            buildingVisual.SetActive(true);

        // Hide unlock button
        if (unlockButton != null)
            unlockButton.gameObject.SetActive(false);
        
        // 🎥 Trigger camera movement on unlock
        if (Camera.main != null)
        {
            previousCamPos = Camera.main.transform.position;
            previousCamRot = Camera.main.transform.rotation;
        }

        if (CameraController.Instance != null && unlockCameraPoint != null)
        {
            StartCoroutine(PlayUnlockCameraSequence());
        }
        AudioManager.Instance.PlayNewBuildingSound();
        GameManager.Instance.ChangeReputation(5);
        GameManager.Instance.UpdateTopUI();
        Debug.Log("✅ Building Unlocked!");
    }

    IEnumerator PlayUnlockCameraSequence()
    {
        // Move to unlock position
        CameraController.Instance.MoveTo(unlockCameraPoint);

        // Wait
        yield return new WaitForSeconds(cameraStayDuration);

        // Return to previous position (NOT default)
        if (CameraController.Instance != null)
        {
            CameraController.Instance.StopAllCoroutines(); // stop current movement
            StartCoroutine(SmoothReturn());
        }
    }

    IEnumerator SmoothReturn()
    {
        Transform cam = Camera.main.transform;

        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * CameraController.Instance.moveSpeed;

            cam.position = Vector3.Lerp(startPos, previousCamPos, t);
            cam.rotation = Quaternion.Lerp(startRot, previousCamRot, t);

            yield return null;
        }
    }

    public void ClosePanel()
    {
        interactionPanel.SetActive(false);
        currentPanel = null;
        CameraController.Instance.MoveToDefault();
    }
}