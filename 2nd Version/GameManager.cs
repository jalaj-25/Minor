using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Stats")]
    public int money = 5000;

    public int totalTeachers = 15;
    public int totalStudents = 155;

    public int freeTeachers = 15;
    public int freeStudents = 155;
    int assignedTeacherCounter = 0;

    public int reputation = 50;
    public int happiness = 70;

    [Header("UI References")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI teacherText;
    public TextMeshProUGUI studentText;
    public TextMeshProUGUI reputationText;
    public TextMeshProUGUI happinessText;
    
    [Header("Timer Settings")]
    public float timeLimit = 120f; // total time
    private float currentTime;
    private bool isGameOver = false;

    [Header("Pause System")]
    public GameObject pausePanel;
    private bool isPaused = false;

    [Header("Timer UI")]
    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel;

    [Header("Star UI")]
    public GameObject starObject;

    //[Header("Text Effect")] 
    //public UI_PopEffect textPopEffect;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadFromPrefs();
        UpdateTopUI();
        currentTime = timeLimit;
        gameOverPanel.SetActive(false);
    }

    // ---------------- UI ----------------
    void Update()
    {
        if (isGameOver) return;

        // ⌨ Press E to toggle pause
        if (Input.GetKeyDown(KeyCode.E))
        {
            TogglePause();
        }

        if (!isPaused)
        {
            RunTimer();
        }
    }
    void RunTimer()
    {
        currentTime -= Time.deltaTime;

        // Update timer UI
        if (timerText != null)
            timerText.text = "Time: " + Mathf.Ceil(currentTime).ToString();

        if (currentTime <= 0)
        {
            currentTime = 0;
            GameOver();
        }
    }
    void GameOver()
    {
        isGameOver = true;

        Debug.Log("GAME OVER");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }
    public void UpdateTopUI()
    {
        if (moneyText != null)
            moneyText.text = "💰 " + money;

        if (teacherText != null)
            teacherText.text = "👨‍🏫 " + freeTeachers + "/" + totalTeachers;

        if (studentText != null)
            studentText.text = "🎓 " + freeStudents + "/" + totalStudents;

        if (reputationText != null)
            reputationText.text = "⭐ " + reputation;

        if (happinessText != null)
            happinessText.text = "😊 " + happiness;
    }

    // ---------------- MONEY ----------------

    public void AddMoney(int amount)
    {
        money += amount;

        if (moneyText != null)
        {
            moneyText.text = "💰 " + money;

            UI_PopEffect pop = moneyText.GetComponent<UI_PopEffect>();
            if (pop != null) pop.PlayPopUpEffect();
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMoneySound();
        }

        // ⭐ SHOW STAR EFFECT
        ShowStarEffect();
    }
    void ShowStarEffect()
    {
        if (starObject == null) return;

        StopAllCoroutines(); // prevent overlap
        StartCoroutine(StarRoutine());
    }

    IEnumerator StarRoutine()
    {
        starObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        starObject.SetActive(false);
    }
    public void SpendMoney(int amount)
    {
        int previousMoney = money;

        money -= amount;
        if (money < 0) money = 0;

        Debug.Log("Money Deducted: ₹" + amount +
                  " | Previous Amount: ₹" + previousMoney +
                  " | New Amount: ₹" + money);

        if (moneyText != null)
        {
            moneyText.text = "💰 " + money;

            UI_PopEffect pop = moneyText.GetComponent<UI_PopEffect>();
            if (pop != null) pop.PlayPopUpEffect();
        }
    }
    // ---------------- REPUTATION ----------------

    public void ChangeReputation(int amount)
    {
        reputation += amount;
        reputation = Mathf.Clamp(reputation, 0, 100);

        if (reputationText != null)
        {
            reputationText.text = "⭐ " + reputation;

            UI_PopEffect pop = reputationText.GetComponent<UI_PopEffect>();
            if (pop != null) pop.PlayPopUpEffect();
        }
    }

    // ---------------- HAPPINESS ----------------

    public void ChangeHappiness(int amount)
    {
        happiness += amount;
        happiness = Mathf.Clamp(happiness, 0, 100);

        if (happinessText != null)
        {
            happinessText.text = "😊 " + happiness;

            UI_PopEffect pop = happinessText.GetComponent<UI_PopEffect>();
            if (pop != null) pop.PlayPopUpEffect();
        }
    }

    // ---------------- STUDENTS ----------------

    public void AddStudents(int amount)
    {
        totalStudents += amount;
        freeStudents += amount;

        if (studentText != null)
        {
            studentText.text = "🎓 " + freeStudents + "/" + totalStudents;

            UI_PopEffect pop = studentText.GetComponent<UI_PopEffect>();
            if (pop != null) pop.PlayPopUpEffect();
        }
    }

    public void RemoveStudents(int amount)
    {
        totalStudents -= amount;
        freeStudents -= amount;

        if (totalStudents < 0) totalStudents = 0;
        if (freeStudents < 0) freeStudents = 0;

        if (studentText != null)
        {
            studentText.text = "🎓 " + freeStudents + "/" + totalStudents;

            UI_PopEffect pop = studentText.GetComponent<UI_PopEffect>();
            if (pop != null) pop.PlayPopUpEffect();
        }
    }

    // ---------------- TEACHERS ----------------

    public void AddTeachers(int amount)
    {
        Debug.Log("AddTeachers CALLED");

        totalTeachers += amount;
        freeTeachers += amount;

        if (teacherText != null)
        {
            teacherText.text = "👨‍🏫 " + freeTeachers + "/" + totalTeachers;

            // 🔥 Try getting pop effect from SAME object
            UI_PopEffect pop = teacherText.GetComponent<UI_PopEffect>();

            if (pop != null) pop.PlayPopUpEffect();
        }
    }

    public void RemoveTeachers(int amount)
    {
        totalTeachers -= amount;
        freeTeachers -= amount;

        if (totalTeachers < 0) totalTeachers = 0;
        if (freeTeachers < 0) freeTeachers = 0;

        if (teacherText != null)
        {
            teacherText.text = "👨‍🏫 " + freeTeachers + "/" + totalTeachers;

            UI_PopEffect pop = teacherText.GetComponent<UI_PopEffect>();

            if (pop == null)
                pop = teacherText.GetComponentInParent<UI_PopEffect>();

            if (pop != null)
                pop.PlayPopUpEffect();
        }
    }

    public void OnTeacherAssigned()
    {
        if (freeTeachers <= 0) return;
        // reduce free teachers (since one got assigned)
        freeTeachers--;

        assignedTeacherCounter++;

        // every 3 assignments → bonus +1 teacher
        if (assignedTeacherCounter >= 3)
        {
            assignedTeacherCounter = 0;

            totalTeachers += 1;
            freeTeachers += 1;

            Debug.Log("Bonus Teacher Gained!");
        }

        UpdateTopUI(); // refresh UI
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        isPaused = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    // 🔁 Restart current level
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 🆕 New Game (same as restart for now)
    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 🏠 Go to Home Scene
    public void LoadHomeScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("AdminScene"); // change name if needed
    }

    void LoadFromPrefs()
    {
        string levelKey = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        money = PlayerPrefs.GetInt(levelKey + "_Money", 5000);

        totalTeachers = PlayerPrefs.GetInt(levelKey + "_Teachers", 15);
        totalStudents = PlayerPrefs.GetInt(levelKey + "_Students", 155);

        freeTeachers = totalTeachers;
        freeStudents = totalStudents;

        reputation = PlayerPrefs.GetInt(levelKey + "_Reputation", 50);
        happiness = PlayerPrefs.GetInt(levelKey + "_Happiness", 70);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadFromPrefs();   // ✅ reload values
        UpdateTopUI();     // ✅ update UI

        currentTime = timeLimit;
        isGameOver = false;

        Time.timeScale = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }
}