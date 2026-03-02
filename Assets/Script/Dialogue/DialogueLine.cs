using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 4)]
    public string text;

    public Sprite sprite;

    [Header("Money Logic")]
    public bool deductMoney;
    public int amountToDeduct;
}
