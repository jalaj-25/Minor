using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingEnterTrigger : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactUI;
    public TextMeshProUGUI interactText;

    [Header("Building Info")]
    public string roomName;
    public string sceneToLoad;
    public string returnPointName; // NEW

    bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.SetString("ReturnPoint", returnPointName);
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactUI.SetActive(true);
            interactText.text = "Press E to Enter " + roomName;

            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactUI.SetActive(false);
            playerInside = false;
        }
    }
}