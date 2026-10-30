using UnityEngine;
using UnityEngine.UIElements;

public class WeakPointScript : MonoBehaviour
{
    //public ParticleSystem enemyDestroyed;

    [Header("Drop")]
    public GameObject scorePickupPrefab; 
    public int pickupDropValue = 50; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //enemyDestroyed.Play();

            SpawnPickup();

            Destroy(gameObject.transform.parent.gameObject, .195f);
        }
    }

    void SpawnPickup()
    {
        if (scorePickupPrefab == null) return;
        Transform parent = transform.parent != null ? transform.parent : transform;
        GameObject go = Instantiate(scorePickupPrefab, parent.position, Quaternion.identity);
        var pickup = go.GetComponent<ScorePickup>();
        if (pickup != null) pickup.scoreValue = pickupDropValue;
    }
}