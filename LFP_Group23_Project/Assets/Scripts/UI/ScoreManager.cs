using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText;

    private int score = 0;
    private int highScore = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); return; }
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // The old scoreText was destroyed with the previous scene.
        // Find the new scene's score text by name.
        var go = GameObject.Find("ScoreText");   // <-- must match the name of your score TMP object
        scoreText = go != null ? go.GetComponent<TextMeshProUGUI>() : null;

        UpdateUI();
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        float mult = (PowerUpManager.Instance != null)
            ? PowerUpManager.Instance.GetMultiplier(PowerUpType.ScoreMultiplier)
            : 1f;

        int scaled = Mathf.RoundToInt(amount * mult);

        score += scaled;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        UpdateUI();
    }

    public int GetScore() => score;

    public void ResetScore()
    {
        score = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
    }
}