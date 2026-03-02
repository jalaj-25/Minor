using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Stats")]
    public int campusLevel = 1;
    public bool gamePaused = false;

    [Header("Moeny UI")]
    public int playerMoney = 100;
    
    [Header("Game Stats")]
    public GameObject uiPanel;
    public GameObject pausePanel;
    public GameObject ShopPanel;
    //public GameObject ShopPanelButton;

    [Header("Teachers")]
    public List<Teacher> hiredTeachers = new List<Teacher>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
            Destroy(gameObject);
    }
    private void Update()
    {
        // 🔥 Press E to Toggle Pause
        if (Input.GetKeyDown(KeyCode.E))
        {
            TogglePause();
        }
    }
    public void AddMoney(int amount)
    {
        playerMoney += amount;
        Debug.Log("Money Added. Current: " + playerMoney);
    }

    public bool DeductMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;
            Debug.Log("Money Deducted. Current: " + playerMoney);
            return true;
        }

        Debug.Log("Not enough money!");
        return false;
    }


    public void LevelUpCampus()
    {
        campusLevel++;
        Debug.Log("Campus Level Up: " + campusLevel);
    }

    public void OpenShopPanel()
    {
        ShopPanel.SetActive(true);
        uiPanel.SetActive(false);

    }

    public void CloseShopPanel()
    {
        ShopPanel.SetActive(false);
        uiPanel.SetActive(true);
    }
    public void TogglePause()
    {
        if (gamePaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        gamePaused = true;

        Time.timeScale = 0f;   // 🔥 Freeze game

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }

        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        gamePaused = false;

        Time.timeScale = 1f;   // 🔥 Resume game

        if (pausePanel != null)
            pausePanel.SetActive(false);
            uiPanel.SetActive(true);

        Debug.Log("Game Resumed");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 🔥 Always reset pause when scene changes
        Time.timeScale = 1f;
        gamePaused = false;

        Debug.Log("Scene Loaded: " + scene.name + " | Game Resumed Automatically");
    }
}