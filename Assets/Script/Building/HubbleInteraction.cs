using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HubbleInteraction : MonoBehaviour
{
    [Header("UI")]
    public GameObject hubbleCanvas;
    public TextMeshProUGUI dialogueText;
    public Image dialogueImage;

    [Header("Dialogues")]
    public DialogueLine[] dialogues;

    [Header("Building")]
    public GameObject hubbleBuilding;

    [Header("Money")]
    public PlayerMoney playerMoney;

    private int index = 0;
    private bool isUnlocked = false;

    private void Start()
    {
        // Hide UI and building at start
        hubbleCanvas.SetActive(false);

        if (hubbleBuilding != null)
            hubbleBuilding.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 🚫 If already unlocked, do NOTHING
        if (isUnlocked) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            OpenDialogue();
        }
    }

    void OpenDialogue()
    {
        index = 0;
        hubbleCanvas.SetActive(true);
        ShowLine();
        Time.timeScale = 0f;
    }

    public void NextDialogue()
    {
        HandleMoneyAndUnlockLogic();

        // If unlocked during this click → instantly close and block forever
        if (isUnlocked)
        {
            CloseDialogue();
            return;
        }

        index++;

        if (index < dialogues.Length)
        {
            ShowLine();
        }
        else
        {
            CloseDialogue();
        }
    }

    void ShowLine()
    {
        dialogueText.text = dialogues[index].text;
        dialogueImage.sprite = dialogues[index].sprite;
    }

    void HandleMoneyAndUnlockLogic()
    {
        DialogueLine line = dialogues[index];

        if (line.deductMoney && !isUnlocked)
        {
            bool success = playerMoney.DeductMoney(line.amountToDeduct);

            if (success)
            {
                isUnlocked = true;
                hubbleBuilding.SetActive(true);
                Debug.Log("Hubble Building Unlocked!");
            }
            else
            {
                dialogueText.text = "Not enough money!";
            }
        }
    }

    void CloseDialogue()
    {
        hubbleCanvas.SetActive(false);
        Time.timeScale = 1f;
        index = 0;
    }
}
