using UnityEngine;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public List<Task> tasks = new List<Task>();
    public PlayerMoney playerMoney;

    private void Awake()
    {
        Instance = this;
    }

    public void CompleteTask(int index)
    {
        if (tasks[index].completed) return;

        tasks[index].completed = true;

        playerMoney.AddMoney(tasks[index].rewardMoney);

        Debug.Log("Task Completed: " + tasks[index].taskName);

        CheckCampusProgress();
    }

    void CheckCampusProgress()
    {
        int completedCount = 0;

        foreach (var t in tasks)
            if (t.completed) completedCount++;

        if (completedCount >= 3)
            GameManager.Instance.LevelUpCampus();
    }
}