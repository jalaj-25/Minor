using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    [Header("Tasks")]
    public List<BuildTask> tasks;
    private BuildTask currentTask;

    [Header("UI")]
    public Slider progressSlider;
    public TextMeshProUGUI taskText;
    private int currentTaskIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (tasks.Count > 0)
        {
            SetTask(0);
        }
        else
        {
            Debug.LogWarning("No tasks assigned!");
        }
    }

    private void Update()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void SetTask(int index)
    {
        if (index >= tasks.Count)
        {
            Debug.Log("🎉 All tasks completed!");
            return;
        }

        currentTaskIndex = index;
        currentTask = tasks[index];

        UpdateUI();
    }

    public void RegisterPlacement(GameObject placedObject)
    {
        Debug.Log("RegisterPlacement called");

        if (currentTask == null)
        {
            Debug.LogError("❌ currentTask is NULL");
            return;
        }

        BuildObject buildObj = placedObject.GetComponent<BuildObject>();

        if (buildObj == null)
        {
            Debug.LogError("❌ NO BuildObject on: " + placedObject.name);
            return;
        }

        Debug.Log("Placed Type: " + buildObj.type);
        Debug.Log("Target Type: " + currentTask.targetType);

        if (buildObj.type == currentTask.targetType)
        {
            Debug.Log("✅ MATCH → Increasing count");

            currentTask.currentAmount++;

            UpdateUI();

            // ⭐⭐⭐ THIS WAS MISSING ⭐⭐⭐
            if (currentTask.currentAmount >= currentTask.requiredAmount)
            {
                CompleteTask();
            }
        }
        else
        {
            Debug.LogWarning("❌ TYPE NOT MATCHING");
        }
    }

    void UpdateUI()
    {
        if (progressSlider != null)
        {
            progressSlider.maxValue = currentTask.requiredAmount;
            progressSlider.value = currentTask.currentAmount;
        }

        if (taskText != null)
        {
            taskText.text = currentTask.taskName + " (" +
                            currentTask.currentAmount + "/" +
                            currentTask.requiredAmount + ")";
        }
    }

    void CompleteTask()
    {
        Debug.Log("🎉 Task Completed!");

        // ✅ Rewards
        GameManager.Instance.ChangeReputation(currentTask.reputationReward);

        if (currentTask.increaseHappiness)
        {
            GameManager.Instance.ChangeHappiness(currentTask.happinessReward);
        }

        // ⭐ MOVE TO NEXT TASK
        currentTaskIndex++;

        if (currentTaskIndex < tasks.Count)
        {
            SetTask(currentTaskIndex);
        }
        else
        {
            Debug.Log("🏁 All Tasks Finished!");
            progressSlider.value = 0;
            taskText.text = "All Tasks Completed!";
        }
    }
}