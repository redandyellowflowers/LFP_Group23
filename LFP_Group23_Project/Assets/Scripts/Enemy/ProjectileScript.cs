using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileScript : MonoBehaviour
{
    public int timeBeforeSelfDestroy = 1;

    public GameObject spawnPoint;

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
        Destroy(gameObject, timeBeforeSelfDestroy);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.transform.position = spawnPoint.transform.position;
            //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            //Destroy(gameObject, .2f);
        }
        else { Destroy(gameObject); }
    }
}
