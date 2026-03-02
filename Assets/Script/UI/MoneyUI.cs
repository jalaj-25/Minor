using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI moneyUI;

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            moneyUI.text = "$" + GameManager.Instance.playerMoney;
        }
    }
}