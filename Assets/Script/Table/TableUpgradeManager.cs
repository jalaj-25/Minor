using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TableUpgradeManager : MonoBehaviour
{
    [Header("Table Models Per Level")]
    public GameObject[] level1Tables;
    public GameObject[] level2Tables;

    [Header("Level Costs")]
    public int cost_Level1 = 5000;
    public int cost_Level2 = 10000;

    [Header("Level 1 Stats")]
    public int capacity_Level1 = 2;
    public float rewardMultiplier_Level1 = 1f;
    public float speedMultiplier_Level1 = 1f;

    [Header("Level 2 Stats")]
    public int capacity_Level2 = 4;
    public float rewardMultiplier_Level2 = 1.2f;
    public float speedMultiplier_Level2 = 1.5f;

    [Header("UI")]
    public GameObject upgradeIcon;
    public GameObject upgradePanel;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI rewardMultiplierText;
    public TextMeshProUGUI speedMultiplierText;

    [Header("Disable Buttons When Panel Opens")]
    public Button button1ToDisable;
    public Button button2ToDisable;

    [Header("References")]
    public PlayerMoney playerMoney;

    private int currentLevel = 1;
    private int currentCost;
    private int currentCapacity;
    private float currentRewardMultiplier;
    private float currentSpeedMultiplier;

    private bool playerInside = false;

    void Start()
    {
        ActivateLevel(1);
        SetLevelStats(1);

        if (upgradeIcon != null)
            upgradeIcon.SetActive(false);

        if (upgradePanel != null)
            upgradePanel.SetActive(false);
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            OpenUpgradePanel();
        }
    }

    // ==============================
    // TRIGGER
    // ==============================

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;

        if (upgradeIcon != null)
            upgradeIcon.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;

        if (upgradeIcon != null)
            upgradeIcon.SetActive(false);
    }

    // ==============================
    // PANEL CONTROL
    // ==============================

    public void OpenUpgradePanel()
    {
        upgradePanel.SetActive(true);
        upgradeIcon.SetActive(false);

        DisableOtherButtons(true);
        UpdateUI();
    }

    public void CloseUpgradePanel()
    {
        upgradePanel.SetActive(false);

        if (playerInside)
            upgradeIcon.SetActive(true);

        DisableOtherButtons(false);
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
            currentCost = cost_Level1;
            currentCapacity = capacity_Level1;
            currentRewardMultiplier = rewardMultiplier_Level1;
            currentSpeedMultiplier = speedMultiplier_Level1;
        }
        else if (level == 2)
        {
            currentCost = cost_Level2;
            currentCapacity = capacity_Level2;
            currentRewardMultiplier = rewardMultiplier_Level2;
            currentSpeedMultiplier = speedMultiplier_Level2;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        levelText.text = "Table Level: " + currentLevel;
        costText.text = "Upgrade Cost: $" + currentCost;
        rewardMultiplierText.text = "Reward Multiplier: x" + currentRewardMultiplier;
        speedMultiplierText.text = "Speed Multiplier: x" + currentSpeedMultiplier;
    }

    public void UpgradeTable()
    {
        if (!playerMoney.DeductMoney(currentCost))
        {
            Debug.Log("Not enough money.");
            return;
        }

        currentLevel++;

        if (currentLevel > 2)
            currentLevel = 2;

        ActivateLevel(currentLevel);
        SetLevelStats(currentLevel);

        Debug.Log("Table upgraded to Level " + currentLevel);
    }

    void ActivateLevel(int level)
    {
        // Level 1 always active
        foreach (GameObject t in level1Tables)
            t.SetActive(true);

        // Level 2 activates extra content
        if (level >= 2)
        {
            foreach (GameObject t in level2Tables)
                t.SetActive(true);
        }
    }
    // ==============================
    // PUBLIC GETTERS (For Receptionist / NPC System)
    // ==============================

    public int GetCapacity()
    {
        return currentCapacity;
    }

    public float GetRewardMultiplier()
    {
        return currentRewardMultiplier;
    }

    public float GetSpeedMultiplier()
    {
        return currentSpeedMultiplier;
    }
}