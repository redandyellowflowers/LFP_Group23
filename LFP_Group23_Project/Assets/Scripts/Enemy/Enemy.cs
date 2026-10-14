using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;

    [Header("Score Values")]
    public int lightHitPoints = 10;
    public int heavyHitPoints = 25;

    [Header("Drop")]
    public GameObject scorePickupPrefab;   // CHANGED: assign in inspector
    public int pickupDropValue = 50;       // CHANGED: points given when collected

    public static System.Action<Enemy> OnEnemyDestroyed;

    private Animator anim;
    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (health <= 0 && !isDead)
        {
            isDead = true;
            SpawnPickup();                 // CHANGED
            OnEnemyDestroyed?.Invoke(this);
            Destroy(gameObject);
        }
    }

    // CHANGED: spawn the pickup at the enemy's position
    void SpawnPickup()
    {
        if (scorePickupPrefab == null) return;
        GameObject go = Instantiate(scorePickupPrefab, transform.position, Quaternion.identity);
        var pickup = go.GetComponent<ScorePickup>();
        if (pickup != null) pickup.scoreValue = pickupDropValue;
    }

    public void TakeDamage(int damage, bool isHeavy = false)
    {
        health -= damage;
        // collecting the dropped pickup, not from hitting.
        Debug.Log((isHeavy ? "Heavy" : "Light") + " damage taken. Health: " + health);
    }
}