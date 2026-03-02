using UnityEngine;

[System.Serializable]
public class Teacher
{
    public string teacherName;
    public string subject;
    public int skillLevel;
    public int salary;
    public float efficiency;

    public GameObject teacherPrefab; // optional
}