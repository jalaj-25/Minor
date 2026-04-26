using UnityEngine;
using UnityEngine.SceneManagement;

public class AdminNavigation : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject settingsPanel;
    public GameObject tutorialPanel;

    // 🔙 Back to Campus
    public void BackToCampus()
    {
        SceneManager.LoadScene("MainScene 1");
    }

    // ⚙️ Open Settings
    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        Time.timeScale = 0f; // pause game
    }

    public void OpenTutorial()
    {
        if(tutorialPanel != null)
            tutorialPanel.SetActive(true);

        Time.timeScale = 0f;
    }
    
    // ❌ Close Settings
    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        Time.timeScale = 1f; // resume game
    }

    public void CloseTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    // 🚪 Exit Game
    public void ExitGame()
    {
        Debug.Log("Game Closed");

        Application.Quit();

        // For Unity Editor (important)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}