using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPointScript : MonoBehaviour
{
    public bool lastLevel = false;

    public GameObject InteractionPrompt;

    public string sceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InteractionPrompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InteractionPrompt.SetActive(true);

            if (Input.GetKey(KeyCode.E) && !lastLevel)
            {
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (Input.GetKey(KeyCode.E) && !lastLevel) { SceneManager.LoadSceneAsync(sceneName); }
        }
    }
}
