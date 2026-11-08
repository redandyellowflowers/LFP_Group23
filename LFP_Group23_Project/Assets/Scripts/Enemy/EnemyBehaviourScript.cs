using UnityEngine;

public class EnemyBehaviourScript : MonoBehaviour
{
    [Header("player")]
    public GameObject player;

    [Header("weapon gameObjects")]
    public GameObject firePoint;
    public GameObject bulletPrefab;

    [Header("values")]
    public float bulletForce = 20f;
    public float fireRate = 1f;
    public int detectionRadius = 10;

    public Rigidbody2D weaponRigidbody;

    [Header("Animation")]
    public Animator animator; 
    private float nextTimeToFire = 0f;
    Vector2 playerPos;
    [HideInInspector]
    public float distance;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;
        playerPos = player.transform.position;
        Aim();
    }

    public void Aim()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance > detectionRadius)
        {
            Vector2 lookDirection = playerPos - weaponRigidbody.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            weaponRigidbody.rotation = angle;
        }
        else if (distance < detectionRadius)
        {
            Vector2 lookDirection = playerPos - weaponRigidbody.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            weaponRigidbody.rotation = angle;

            Shooting();
        }
    }

    public void Shooting()
    {
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            if (animator != null)
            {
                animator.SetTrigger("shoot");
            }

            GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.transform.up * bulletForce, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, detectionRadius);
    }
}