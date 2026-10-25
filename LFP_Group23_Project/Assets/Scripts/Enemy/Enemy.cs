using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;

    [Header("Drop")]
    public GameObject scorePickupPrefab;
    public int pickupDropValue = 50;

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
            SpawnPickup();
            Destroy(gameObject);
        }
    }

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
        Debug.Log((isHeavy ? "Heavy" : "Light") + " damage taken. Health: " + health);
    }
}