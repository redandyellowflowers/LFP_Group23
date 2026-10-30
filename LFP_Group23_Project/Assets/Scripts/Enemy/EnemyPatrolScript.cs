using UnityEngine;
using UnityEngine.UIElements;

public class EnemyPatrolScript : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;

    public float speed;

    private Rigidbody2D rb;
    private Transform currentPoint;

    [Header("Drop")]
    public GameObject scorePickupPrefab;
    public int pickupDropValue = 50;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPoint = pointB.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint .position - transform.position;
        if (currentPoint == pointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < .5f && currentPoint == pointB.transform)
        {
            currentPoint = pointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < .5f && currentPoint == pointA.transform)
        {
            currentPoint = pointB.transform;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }


    void SpawnPickup()
    {
        if (scorePickupPrefab == null) return;
        GameObject go = Instantiate(scorePickupPrefab, transform.position, Quaternion.identity);
        var pickup = go.GetComponent<ScorePickup>();
        if (pickup != null) pickup.scoreValue = pickupDropValue;
    }

}
