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

        GameObject DamageParticle = Instantiate(damageParticle, gameObject.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        Destroy(DamageParticle, 1f);

        Destroy(gameObject, timeBeforeSelfDestroy);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.transform.position = spawnPoint.transform.position;

            FindAnyObjectByType<AudioManagerScript>().Play("Respawn");

            //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            //Destroy(gameObject, .2f);
        }
        else { Destroy(gameObject); }
    }
}
