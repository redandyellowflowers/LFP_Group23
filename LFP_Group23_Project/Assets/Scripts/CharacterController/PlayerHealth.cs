using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    // Armor absorbs a hit without losing health. Each hit consumes 1 armor.
    public int armor = 0;

    // Brief invulnerability after taking a hit
    public float invulnDuration = 1f;
    private float invulnTimer = 0f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (invulnTimer > 0) invulnTimer -= Time.deltaTime;
    }

    public void AddArmor(int amount)
    {
        armor += amount;
        Debug.Log("Armor: " + armor);
    }

    public bool TakeDamage(int amount)
    {
        if (invulnTimer > 0) return false;
        if (amount <= 0) return false;

        // Armor absorbs the hit entirely
        if (armor > 0)
        {
            armor--;
            invulnTimer = invulnDuration;
            Debug.Log("Armor absorbed the hit! Armor left: " + armor);
            return false;
        }

        currentHealth -= amount;
        invulnTimer = invulnDuration;
        Debug.Log("Player hit! HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            // Restart the scene (matches your existing ProjectileScript behaviour)
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        return true;
    }
}