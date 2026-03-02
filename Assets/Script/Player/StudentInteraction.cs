using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StudentInteraction : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogueCanvas;
    public TextMeshProUGUI dialogueText;
    public Image speakerImage;

    [Header("Conversation")]
    public DialogueLine[] conversation;

    [Header("Money (Optional)")]
    public PlayerMoney playerMoney;

    private int index = 0;
    private bool conversationActive = false;

    private void Start()
    {
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);
    }

    // 🔥 ONLY called manually by StudentsController
    public void StartConversation()
    {
        if (conversation == null || conversation.Length == 0) return;

        index = 0;
        conversationActive = true;

        dialogueCanvas.SetActive(true);
        ShowLine();
    }

    // 🔘 CALLED BY NEXT BUTTON
    public void NextLine()
    {
        if (!conversationActive) return;

        HandleMoneyLogic();

        index++;

        if (index < conversation.Length)
        {
            ShowLine();
        }
        else
        {
            EndConversation();
        }
    }

    void ShowLine()
    {
        DialogueLine line = conversation[index];
        dialogueText.text = line.text;
        speakerImage.sprite = line.sprite;
    }

    void HandleMoneyLogic()
    {
        DialogueLine line = conversation[index];

        if (line.deductMoney && playerMoney != null)
            playerMoney.DeductMoney(line.amountToDeduct);
    }

    void EndConversation()
    {
        conversationActive = false;
        dialogueCanvas.SetActive(false);
        index = 0;
    }
}
