using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class ProjectileScript : MonoBehaviour
{
    private GameObject player;

    public int timeBeforeSelfDestroy = 1;

    public GameObject spawnPoint;

    public GameObject damageParticle;

    private void Awake()
    {
        spawnPoint = GameObject.Find("SpawnPoint");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        //transform.right = direction;

        Destroy(gameObject, timeBeforeSelfDestroy);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Check if the player has the PlayerHealth script (where armor lives)
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();

            // If they have armor, absorb the hit and DO NOT teleport
            if (health != null && health.armor > 0)
            {
                health.armor--; // Remove 1 armor

                // Play a sound or spawn particle to show armor broke, 
                // but for now we'll just destroy the bullet and skip the teleport.
                Destroy(gameObject);
                return;
            }

            // Only runs if armor is 0
            collision.gameObject.transform.position = spawnPoint.transform.position;

            FindAnyObjectByType<AudioManagerScript>().Play("Respawn");

            GameObject DamageParticle = Instantiate(damageParticle, gameObject.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            Destroy(DamageParticle, 1f);

            Destroy(gameObject); // (Added this so the bullet goes away)
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
