using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyTracker : MonoBehaviour
{
    [Tooltip("Leave empty to load the next scene in build order.")]
    public string nextSceneName = "";

    [Tooltip("Delay (seconds) after the last enemy dies before loading.")]
    public float delayBeforeNextLevel = 1.5f;

    private int enemiesAlive = 0;
    private bool levelComplete = false;
    private bool registered = false;

    void OnEnable()
    {
        Enemy.OnEnemyDestroyed += HandleEnemyDestroyed;
    }

    void OnDisable()
    {
        Enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
    }

    void Start()
    {
        // Wait one frame so all enemies' Awake/Start have run
        StartCoroutine(CountEnemiesNextFrame());
    }

    private System.Collections.IEnumerator CountEnemiesNextFrame()
    {
        yield return null; // wait one frame

        enemiesAlive = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        registered = true;
        Debug.Log("Enemies in level: " + enemiesAlive);

        if (enemiesAlive == 0)
            Debug.LogWarning("No enemies found in this scene. Level will never complete.");
    }

    private void HandleEnemyDestroyed(Enemy e)
    {
        if (!registered) return;       // ignore any kills before we counted
        if (levelComplete) return;

        enemiesAlive--;
        Debug.Log("Enemy killed. Remaining: " + enemiesAlive);

        if (enemiesAlive <= 0)
        {
            levelComplete = true;
            Invoke(nameof(LoadNextLevel), delayBeforeNextLevel);
        }
    }

    private void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(nextIndex);
            else
                Debug.Log("No more levels — you beat the game!");
        }
    }
}