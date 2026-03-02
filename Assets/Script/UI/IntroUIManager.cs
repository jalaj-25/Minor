using UnityEngine;
using TMPro;

public class IntroUIManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject introText;
    public GameObject nextButton;
    public GameObject namePromptText;
    public TMP_InputField nameInputField;

    [Header("Intro Text (Editable in Inspector)")]
    [TextArea(3, 6)]
    public string introDialogue;
    public GameObject introPanel;
    public GameObject player;

    private void Start()
    {
        // Pause game
        Time.timeScale = 0f;

        // Set intro text from inspector
        introText.GetComponent<TextMeshProUGUI>().text = introDialogue;

        // Initial state
        nameInputField.gameObject.SetActive(false);
        namePromptText.SetActive(false);
    }

    // Called on Next Button click
    public void OnNextClicked()
    {
        introText.SetActive(false);
        nextButton.SetActive(false);

        namePromptText.SetActive(true);
        nameInputField.gameObject.SetActive(true);

        nameInputField.ActivateInputField();
    }

    private void Update()
    {
        // Detect Enter key after name is typed
        if (nameInputField.gameObject.activeSelf &&
            Input.GetKeyDown(KeyCode.Return))
        {
            SubmitName();
        }
    }

    void SubmitName()
    {
        string playerName = nameInputField.text;

        if (string.IsNullOrWhiteSpace(playerName))
            return;

        Debug.Log("Player Name: " + playerName);

        PlayerPrefs.SetString("PlayerName", playerName);

        // Resume game
        Time.timeScale = 1f;

        // 🔥 THIS is the real fix
        introPanel.SetActive(false);

        // Enable player if needed
        if (player != null)
            player.SetActive(true);
    }


}
