using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ReceptionistManager : MonoBehaviour
{
    [Header("References")]
    public PlayerMoney playerMoney;
    public GameObject receptionistModel;
    public ReceptionTriggerLogic receptionLogic;

    [Header("UI")]
    public GameObject receptionistPanel;
    public GameObject receptionistIcon;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI rewardText;

    [Header("Buttons To Disable When Panel Opens")]
    public Button button1ToDisable;
    public Button button2ToDisable;

    [Header("Level 1 Stats")]
    public float workTime_Level1 = 2f;
    public int reward_Level1 = 25;
    public int cost_Level1 = 50;

    [Header("Level 2 Stats")]
    public float workTime_Level2 = 1f;
    public int reward_Level2 = 50;
    public int cost_Level2 = 100;

    private int currentLevel = 1;
    private float currentWorkTime;
    private int currentRewardValue;
    private int currentCost;

    private bool receptionistHired = false;
    private bool playerInside = false;

    void Start()
    {
        if (receptionistModel != null)
            receptionistModel.SetActive(false);

        if (receptionistPanel != null)
            receptionistPanel.SetActive(false);

        if (receptionistIcon != null)
            receptionistIcon.SetActive(false);

        SetLevelStats(currentLevel);
    }

    // ==============================
    // TRIGGER DETECTION
    // ==============================

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;

        if (receptionistIcon != null)
            receptionistIcon.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;

        if (receptionistIcon != null)
            receptionistIcon.SetActive(false);
    }

    // ==============================
    // PANEL CONTROL
    // ==============================

    public void OpenReceptionPanel()
    {
        if (receptionistPanel != null)
            receptionistPanel.SetActive(true);

        if (receptionistIcon != null)
            receptionistIcon.SetActive(false);

        DisableOtherButtons(true);   // 🔥 Disable other buttons

        UpdateUI();
    }

    public void CloseReceptionPanel()
    {
        if (receptionistPanel != null)
            receptionistPanel.SetActive(false);

        if (playerInside && receptionistIcon != null)
            receptionistIcon.SetActive(true);

        DisableOtherButtons(false);  // 🔥 Enable buttons again
    }

    void DisableOtherButtons(bool disable)
    {
        if (button1ToDisable != null)
            button1ToDisable.interactable = !disable;

        if (button2ToDisable != null)
            button2ToDisable.interactable = !disable;
    }

    // ==============================
    // LEVEL SYSTEM
    // ==============================

    void SetLevelStats(int level)
    {
        if (level == 1)
        {
            currentWorkTime = workTime_Level1;
            currentRewardValue = reward_Level1;
            currentCost = cost_Level1;
        }
        else if (level == 2)
        {
            currentWorkTime = workTime_Level2;
            currentRewardValue = reward_Level2;
            currentCost = cost_Level2;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (statsText != null)
            statsText.text = "Work Time: " + currentWorkTime + "s";

        if (rewardText != null)
            rewardText.text = "Reward: $" + currentRewardValue;

        if (costText != null)
            costText.text = "Upgrade Cost: $" + currentCost;
    }

    // ==============================
    // HIRE / UPGRADE
    // ==============================

    public void HireOrUpgrade()
    {
        if (!playerMoney.DeductMoney(currentCost))
        {
            Debug.Log("Not enough money.");
            return;
        }

        if (!receptionistHired)
        {
            receptionistHired = true;

            if (receptionistModel != null)
                receptionistModel.SetActive(true);

            StartCoroutine(AutoServe());
        }
        else
        {
            currentLevel++;

            if (currentLevel > 2)
                currentLevel = 2;

            SetLevelStats(currentLevel);
        }
    }

    // ==============================
    // AUTO SERVE SYSTEM
    // ==============================

    IEnumerator AutoServe()
    {
        while (receptionistHired)
        {
            yield return new WaitForSeconds(currentWorkTime);

            if (receptionLogic != null && receptionLogic.CanServe())
            {
                receptionLogic.AutoServeNPC();
                playerMoney.AddMoney(currentRewardValue);
            }
        }
    }
}