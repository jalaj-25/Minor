using UnityEngine;
using TMPro;

public class PlayerMoney : MonoBehaviour
{
    public int money = 100;
    public TextMeshProUGUI moneyText;
    public delegate void MoneyChanged(int newAmount);
    public event MoneyChanged OnMoneyChanged;
    private void Start() 
    {
        UpdateUI();
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public bool DeductMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateUI();
            return true;
        }

        Debug.Log("Not enough money!");
        return false;
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        OnMoneyChanged?.Invoke(money);
        if (moneyText != null)
            moneyText.text = "$ " + money;
    }
}
