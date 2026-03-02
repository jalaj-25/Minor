using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance;

    [Header("Panels")]
    public GameObject questionPanel;
    public GameObject reportPanel;

    [Header("Question UI")]
    public TextMeshProUGUI questionText;
    public Button[] optionButtons;

    [Header("Report UI")]
    public TextMeshProUGUI scoreText;
    public Transform reportContent;
    public GameObject reportItemPrefab;

    [Header("Questions")]
    public QuestionData[] questions;

    private QuestionData[] currentTestQuestions;
    private int currentQuestionIndex = 0;
    private int[] playerAnswers;   // stores selected answers

    void Awake()
    {
        Instance = this;
    }

    // =========================
    // START TEST
    // =========================
    public void StartTest()
    {
        reportPanel.SetActive(false);
        questionPanel.SetActive(true);
        //Time.timeScale = 0f;

        currentTestQuestions = questions;
        playerAnswers = new int[currentTestQuestions.Length];

        for (int i = 0; i < playerAnswers.Length; i++)
            playerAnswers[i] = -1;

        currentQuestionIndex = 0;

        LoadQuestion();
    }

    // =========================
    // LOAD QUESTION
    // =========================
    void LoadQuestion()
    {
        QuestionData q = currentTestQuestions[currentQuestionIndex];

        questionText.text = q.questionText;

        // Hide all buttons first
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(false);
        }

        // MCQ TYPE
        if (q.questionType == QuestionType.MCQ)
        {
            for (int i = 0; i < q.options.Length; i++)
            {
                optionButtons[i].gameObject.SetActive(true);

                int index = i;

                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text =
                    q.options[i];

                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => SelectAnswer(index));
            }
        }

        // TRUE FALSE TYPE
        else if (q.questionType == QuestionType.TrueFalse)
        {
            optionButtons[0].gameObject.SetActive(true);
            optionButtons[1].gameObject.SetActive(true);

            optionButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = "True";
            optionButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = "False";

            optionButtons[0].onClick.RemoveAllListeners();
            optionButtons[1].onClick.RemoveAllListeners();

            optionButtons[0].onClick.AddListener(() => SelectAnswer(0));
            optionButtons[1].onClick.AddListener(() => SelectAnswer(1));
        }
    }

    // =========================
    // STORE ANSWER
    // =========================
    public void SelectAnswer(int index)
    {
        Debug.Log("Selected Option: " + index);
        playerAnswers[currentQuestionIndex] = index;
    }

    // =========================
    // NEXT QUESTION
    // =========================
    public void NextQuestion()
    {
        Debug.Log("Next Question");
        if (currentQuestionIndex < currentTestQuestions.Length - 1)
        {
            currentQuestionIndex++;
            LoadQuestion();
        }
    }

    // =========================
    // PREVIOUS QUESTION
    // =========================
    public void PreviousQuestion()
    {
        Debug.Log("Previous Question");
        if (currentQuestionIndex > 0)
        {
            currentQuestionIndex--;
            LoadQuestion();
        }
    }

    // =========================
    // SUBMIT TEST
    // =========================
    public void SubmitTest()
    {
        Debug.Log("Submit Question");
        questionPanel.SetActive(false);
        ShowReport();
    }

    // =========================
    // SHOW REPORT
    // =========================
    void ShowReport()
    {
        reportPanel.SetActive(true);

        // Clear old report items
        foreach (Transform child in reportContent)
        {
            Destroy(child.gameObject);
        }

        int correctCount = 0;
        int totalMoney = 0;

        for (int i = 0; i < currentTestQuestions.Length; i++)
        {
            QuestionData q = currentTestQuestions[i];

            bool isCorrect = false;

            if (q.questionType == QuestionType.MCQ)
            {
                isCorrect = playerAnswers[i] == q.correctAnswerIndex;
            }
            else if (q.questionType == QuestionType.TrueFalse)
            {
                bool selectedBool = playerAnswers[i] == 0;
                isCorrect = selectedBool == q.correctTrueFalse;
            }

            if (isCorrect)
            {
                correctCount++;
                totalMoney += q.rewardMoney;
            }

            CreateReportItem(q, playerAnswers[i], isCorrect);
        }

        scoreText.text = "Score: " + correctCount + "/" + currentTestQuestions.Length;

        GameManager.Instance.AddMoney(totalMoney);
    }

    // =========================
    // CREATE REPORT ITEM
    // =========================
    void CreateReportItem(QuestionData q, int playerAnswer, bool isCorrect)
    {
        GameObject item = Instantiate(reportItemPrefab, reportContent);

        item.transform.Find("QuestionText").GetComponent<TextMeshProUGUI>().text =
            q.questionText;

        string playerAnswerText = "Not Answered";

        if (playerAnswer >= 0)
        {
            if (q.questionType == QuestionType.MCQ)
                playerAnswerText = q.options[playerAnswer];
            else
                playerAnswerText = playerAnswer == 0 ? "True" : "False";
        }

        string correctAnswerText;

        if (q.questionType == QuestionType.MCQ)
            correctAnswerText = q.options[q.correctAnswerIndex];
        else
            correctAnswerText = q.correctTrueFalse ? "True" : "False";

        item.transform.Find("PlayerAnswerText").GetComponent<TextMeshProUGUI>().text =
            "Your Answer: " + playerAnswerText;

        item.transform.Find("CorrectAnswerText").GetComponent<TextMeshProUGUI>().text =
            "Correct Answer: " + correctAnswerText;

        item.transform.Find("ExplanationText").GetComponent<TextMeshProUGUI>().text =
            "Explanation: " + q.explanation;

        item.transform.Find("ResultText").GetComponent<TextMeshProUGUI>().text =
            isCorrect ? "Correct ✅" : "Wrong ❌";
    }

    // =========================
    // CLOSE REPORT
    // =========================
    public void CloseReport()
    {
        reportPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}