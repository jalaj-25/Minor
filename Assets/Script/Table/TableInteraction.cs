using UnityEngine;

public class TableInteraction : MonoBehaviour
{
    public GameObject upgradePanel;
    public TableUpgradeManager tableManager;

    private bool playerInRange;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
                upgradePanel.SetActive(true);
                Time.timeScale = 0f; // Pause game
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}