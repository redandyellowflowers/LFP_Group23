using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyTracker : MonoBehaviour
{
    [Tooltip("Leave empty to load the next scene in build order.")]
    public string nextSceneName = "";

    [Tooltip("Delay (seconds) after the last enemy dies before loading.")]
    public float delayBeforeNextLevel = 2f;

    private int enemiesAlive = 0;
    private bool levelComplete = false;
    private bool registered = false;
    public static string NextLevelAfterShop = "";

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
            //Invoke(nameof(LoadNextLevel), delayBeforeNextLevel);
        }
    }

    private void LoadNextLevel()
    {
        // Store the *name* of the real next level so ShopScene can load it.
        // If nextSceneName is set, use it. Otherwise, this is the last level.
        NextLevelAfterShop = nextSceneName;

        if (string.IsNullOrEmpty(NextLevelAfterShop))
        {
            Debug.Log("[EnemyTracker] No next level set — you beat the game!");
            return;   // don't load the shop if there's nowhere to go after
        }

        SceneManager.LoadScene("ShopScene");
    }
}