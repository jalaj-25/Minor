using UnityEngine;

public class QuestionTrigger : MonoBehaviour
{
    public GameObject interactionPanel;
    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            interactionPanel.SetActive(false);

            if (QuestionManager.Instance != null)
            {
                QuestionManager.Instance.StartTest();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            interactionPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            interactionPanel.SetActive(false);
        }
    }
}