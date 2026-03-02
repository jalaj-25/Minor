using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public int currentMoney = 100;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool DeductMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            Debug.Log("Money deducted. Remaining: " + currentMoney);
            return true;
        }

        Debug.Log("Not enough money!");
        return false;
    }
}
