using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDataInitializer : MonoBehaviour
{
    [Header("Default Values for This Scene")]
    public int startMoney = 5000;
    public int totalTeachers = 15;
    public int totalStudents = 155;
    public int reputation = 50;
    public int happiness = 70;

    void Awake()
    {
        string levelKey = SceneManager.GetActiveScene().name;

        // Only set values if not already saved
        if (!PlayerPrefs.HasKey(levelKey + "_Money"))
        {
            PlayerPrefs.SetInt(levelKey + "_Money", startMoney);
            PlayerPrefs.SetInt(levelKey + "_Teachers", totalTeachers);
            PlayerPrefs.SetInt(levelKey + "_Students", totalStudents);
            PlayerPrefs.SetInt(levelKey + "_Reputation", reputation);
            PlayerPrefs.SetInt(levelKey + "_Happiness", happiness);

            PlayerPrefs.Save();
        }
    }
}