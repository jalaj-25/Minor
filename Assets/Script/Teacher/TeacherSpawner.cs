using System.Collections.Generic;
using UnityEngine;

public class TeacherSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;   // Multiple spawn locations
    public GameObject teacherPrefab;
    public GameObject teacherPanel;
    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager Missing in Scene!");
            return;
        }
        SpawnTeachers();
    }

    public void OpenTeacherPanel()
    {
        teacherPanel.SetActive(true);
    }

    public void CloseTeacherPanel()
    {
        teacherPanel.SetActive(false);
    }

    void SpawnTeachers()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager is NULL!");
            return;
        }

        if (GameManager.Instance.hiredTeachers == null)
        {
            Debug.LogError("HiredTeachers list is NULL!");
            return;
        }

        List<Teacher> hired = GameManager.Instance.hiredTeachers;

        for (int i = 0; i < hired.Count; i++)
        {
            if (i < spawnPoints.Length)
            {
                Instantiate(
                    teacherPrefab,
                    spawnPoints[i].position,
                    spawnPoints[i].rotation
                );
            }
        }
    }
}