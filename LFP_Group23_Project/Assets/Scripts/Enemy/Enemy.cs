using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public int health;
    //  public float speed;

    [Header("Score Values")]
    public int lightHitPoints = 10;   // points awarded for a light hit
    public int heavyHitPoints = 25;   // points awarded for a heavy hit

    // NEW: notified when any enemy is destroyed
    public static System.Action<Enemy> OnEnemyDestroyed;

    private Animator anim;
    private bool isDead = false;   // NEW: guards against double-firing

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (health <= 0 && !isDead)
        {
            isDead = true;
            OnEnemyDestroyed?.Invoke(this);
            Destroy(gameObject);
        }
    }

    // isHeavy tells us whether the attack was a light or heavy one
    public void TakeDamage(int damage, bool isHeavy = false)
    {
        health -= damage;

        // Award points to the player for landing a hit
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(isHeavy ? heavyHitPoints : lightHitPoints);
        }

        Debug.Log((isHeavy ? "Heavy" : "Light") + " damage taken. Health: " + health);
    }
}