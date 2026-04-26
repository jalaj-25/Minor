using UnityEngine;
using TMPro;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    public int currentDay = 1;
    public TMP_Text dayText;

    public EventManager eventManager;

    public float dayDuration = 10f;
    float timer;
    int studentsLeavingRate = 1; // starts with 1 student leaving
    int teacherLeavingRate = 1;
    float pendingReputationLoss = 0f;
    float pendingHappinessLoss = 0f;
    public float impactSpeed = 2f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateDayUI();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= dayDuration)
        {
            timer = 0;
            NextDay();
        }
        ApplyImpactOverTime();
    }

    void NextDay()
    {
        currentDay++;

        UpdateDayUI();

        int dailyIncome = 0;

        if (eventManager != null)
        {
            dailyIncome = eventManager.GenerateDailyIncome();
            eventManager.NextDay();
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayDayChangeSound();
        }

        HandleStudentLeaving(); // 👈 ADD THIS
        HandleTeacherLeaving();
        HandleAutoHireTeachers();
        Debug.Log("Day " + currentDay + " started | Daily Income: ₹" + dailyIncome);
    }

    void UpdateDayUI()
    {
        if (dayText != null)
            dayText.text = "Day " + currentDay;
    }
    void HandleStudentLeaving()
    {
        if (GameManager.Instance == null) return;

        int free = GameManager.Instance.freeStudents;

        if (free <= 0) return;

        int leaving = Mathf.Min(free, studentsLeavingRate);

        GameManager.Instance.RemoveStudents(leaving);

        Debug.Log("Students Left: " + leaving);

        // 🔥 IMPACT
        ApplyStudentLeavingImpact(leaving);

        studentsLeavingRate++;
    }

    void ApplyStudentLeavingImpact(int leavingCount)
    {
        if (GameManager.Instance == null) return;

        float repLoss = Mathf.Clamp(leavingCount, 1, 5); // ✅ max 5
        float happyLoss = leavingCount * 2;

        pendingReputationLoss += repLoss;
        pendingHappinessLoss += happyLoss;
    }

    void HandleTeacherLeaving()
    {
        if (GameManager.Instance == null) return;

        int free = GameManager.Instance.freeTeachers;

        if (free <= 0) return;

        int leaving = Mathf.Min(free, teacherLeavingRate);

        GameManager.Instance.RemoveTeachers(leaving);

        Debug.Log("Teachers Left: " + leaving);
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayTeacherLeaveSound();
        }
        
        // 🔥 IMPACT BASED ON HOW MANY LEFT
        ApplyTeacherLeavingImpact(leaving);

        teacherLeavingRate++;
    }

    void HandleAutoHireTeachers()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.totalTeachers <= 0)
        {
            int cost = 100;
            int hireCount = 4;

            if (GameManager.Instance.money >= cost)
            {
                GameManager.Instance.SpendMoney(cost);
                GameManager.Instance.AddTeachers(hireCount);

                Debug.Log("Auto Hired 4 Teachers for ₹100");
            }
            else
            {
                Debug.Log("Not enough money to hire teachers!");
            }
        }
    }

    void ApplyTeacherLeavingImpact(int leavingCount)
    {
        if (GameManager.Instance == null) return;

        float repLoss = Mathf.Clamp(leavingCount, 1, 5); // ✅ max 5
        float happyLoss = leavingCount * 3;

        pendingReputationLoss += repLoss;
        pendingHappinessLoss += happyLoss;
    }

    void ApplyImpactOverTime()
    {
        if (GameManager.Instance == null) return;

        // Reputation
        if (pendingReputationLoss > 0)
        {
            float step = impactSpeed * Time.deltaTime;
            GameManager.Instance.ChangeReputation(-(int)step);
            pendingReputationLoss -= step;
        }

        // Happiness
        if (pendingHappinessLoss > 0)
        {
            float step = impactSpeed * Time.deltaTime;
            GameManager.Instance.ChangeHappiness(-(int)step);
            pendingHappinessLoss -= step;
        }
    }
}