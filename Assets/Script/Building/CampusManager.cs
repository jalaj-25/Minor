using UnityEngine;

public class CampusManager : MonoBehaviour
{
    public static CampusManager Instance;

    public GameObject[] campusLevels;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateCampusVisual(int level)
    {
        for (int i = 0; i < campusLevels.Length; i++)
        {
            campusLevels[i].SetActive(i == level - 1);
        }
    }
}