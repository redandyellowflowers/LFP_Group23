using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damageToPlayer = 10;
    public int damageToEnemy = 15;
    public float lifeTime = 5f;

    private Vector2 direction;
    private bool isDeflected = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = transform.right;
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (rb != null) rb.linearVelocity = direction * speed;
        else transform.Translate(direction * speed * Time.deltaTime);
    }

    // Called by the player's heavy attack
    public void Deflect()
    {
        if (isDeflected) return; // already deflected, ignore

        isDeflected = true;
        direction = -direction; // reverse it
        speed *= 1.5f;

        // Flip sprite so it visually points back
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;

        Debug.Log("Projectile deflected!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDeflected)
        {
            // Deflected projectiles damage enemies
            Enemy e = other.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(damageToEnemy, true);
                Destroy(gameObject);
            }
        }
        else
        {
            // Normal projectiles damage the player
            if (other.CompareTag("Player"))
            {
                // player health here
                // other.GetComponent<PlayerHealth>()?.TakeDamage(damageToPlayer);
                Destroy(gameObject);
            }
        }
    }
}