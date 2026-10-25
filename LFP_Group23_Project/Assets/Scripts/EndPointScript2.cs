using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPointScript2 : MonoBehaviour
{
    [Tooltip("Scene loaded when the player touches this endpoint.")]
    public string shopSceneName = "ShopScene";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(shopSceneName);
        }
    }
}