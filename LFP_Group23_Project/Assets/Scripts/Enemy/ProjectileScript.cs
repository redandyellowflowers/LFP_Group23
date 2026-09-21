using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, .75f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            //Destroy(gameObject, .2f);
        }
        else { Destroy(gameObject); }
    }
}
