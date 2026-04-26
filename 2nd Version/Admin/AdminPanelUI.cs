using UnityEngine;
using TMPro;

public class AdminPanelUI : MonoBehaviour
{
    public TMP_Text moneyText;
    public TMP_Text studentText;
    public TMP_Text teacherText;
    public TMP_Text reputationText;

    void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        moneyText.text = "Money: ₹" + GameManager.Instance.money;
        studentText.text = "Students: " + GameManager.Instance.freeStudents;
        teacherText.text = "Teachers: " + GameManager.Instance.freeTeachers;
        reputationText.text = "Reputation: " + GameManager.Instance.reputation;
    }
}