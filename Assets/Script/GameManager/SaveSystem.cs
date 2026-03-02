using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public PlayerMoney playerMoney;

    public void SaveGame()
    {
        PlayerPrefs.SetInt("Money", playerMoney.money);
        PlayerPrefs.SetInt("CampusLevel", GameManager.Instance.campusLevel);
    }

    public void LoadGame()
    {
        playerMoney.money = PlayerPrefs.GetInt("Money", 100);
        GameManager.Instance.campusLevel = PlayerPrefs.GetInt("CampusLevel", 1);
    }
}