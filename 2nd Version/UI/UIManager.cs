using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI studentText;
    public TextMeshProUGUI teacherText;
    public TextMeshProUGUI moneyText;

    void Awake()
    { 
        Instance = this;
    }

    public void UpdateUI(int students, int teachers, int money)
    {
        studentText.text = "Students: " + students;
        teacherText.text = "Teachers: " + teachers;
        moneyText.text = "$ " + money.ToString("N0");
    }
}