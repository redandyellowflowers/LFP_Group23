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

    public bool IsDeflected => isDeflected;

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

    public void Deflect()
    {
        Deflect(-direction);
    }

    public void Deflect(Vector2 newDirection)
    {
        if (isDeflected) return; 

        isDeflected = true;
        direction = newDirection.normalized; 
        speed *= 1.5f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = new Color(1f, 0.6f, 0f, 1f);

        Debug.Log("Projectile deflected!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDeflected)
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null)
            {
                direction = new Vector2(-direction.x, direction.y); 
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
            return;
        }

        if (other.CompareTag("Player"))
        {
            var ph = other.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(damageToPlayer);
            Destroy(gameObject);
        }
    }
}