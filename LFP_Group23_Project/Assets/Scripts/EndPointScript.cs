using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPointScript : MonoBehaviour
{
    public bool lastLEvel = false;

    public string sceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Winner");

            if (!lastLEvel)
            { SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1); }
            else { SceneManager.LoadSceneAsync(sceneName); }
        }
    }
}
