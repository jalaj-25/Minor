using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestion", menuName = "DSA/Question")]
public class QuestionData : ScriptableObject
{
    [TextArea(3, 5)]
    public string questionText;

    public QuestionType questionType;

    public string[] options;   // For MCQ
    public int correctAnswerIndex;

    public bool correctTrueFalse;

    public int rewardMoney;

    [TextArea(2, 4)]
    public string explanation;
}