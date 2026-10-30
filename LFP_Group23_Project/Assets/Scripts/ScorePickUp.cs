using UnityEngine;

public class ScorePickup : MonoBehaviour
{
    public int scoreValue = 10;
    public float pickupRadius = 1.5f;    
    public float moveSpeed = 6f;
    public float lifetime = 15f;

    private Transform player;

    void Start()
    {
        var p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist < pickupRadius)
        {
            // Move toward the player; collect when very close
            transform.position = Vector2.MoveTowards(
                transform.position, player.position, moveSpeed * Time.deltaTime);

            if (dist < 0.2f)
            {
                if (ScoreManager.Instance != null)
                    ScoreManager.Instance.AddScore(scoreValue);
                Destroy(gameObject);
            }
        }
    }
}