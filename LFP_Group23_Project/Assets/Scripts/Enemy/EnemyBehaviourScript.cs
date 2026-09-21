using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviourScript : MonoBehaviour
{
    [Header("player")]
    public GameObject player;

    [HideInInspector]
    public bool hasLineOfSight = false;

    [Header("weapon gameObjects")]
    public GameObject firePoint;
    public GameObject bulletPrefab;

    [Header("values")]
    public float bulletForce = 20f;

    private float nextTimeToFire = 0f;
    public float fireRate = 1f;

    public int detectionRadius = 10;

    public Rigidbody2D weaponRigidbody;

    Vector2 playerPos;

    [HideInInspector]
    public float distance;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;

        if (player != null)
        {
            Aim();
            Shooting();
        }
    }

    public void Aim()
    {
        if (player != null)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance < detectionRadius)
            {
                Vector2 lookDirection = playerPos - weaponRigidbody.position;
                float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
                weaponRigidbody.rotation = angle;
            }
        }
    }

    public void Shooting()
    {
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

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
