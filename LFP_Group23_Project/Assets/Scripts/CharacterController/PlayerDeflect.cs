using System.Collections.Generic;
using UnityEngine;

public class PlayerDeflect : MonoBehaviour
{
    [Header("Destroy Bullets")]
    [Tooltip("Radius around the cursor where bullets get destroyed.")]
    public float destroyRadius = 0.75f;

    [Tooltip("What layers count as projectiles.")]
    public LayerMask whatIsProjectiles;

    [Header("Reward")]
    public int scorePerBullet = 25;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(1)) return;   // RIGHT mouse button

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Collider2D[] hits = Physics2D.OverlapCircleAll(mouseWorld, destroyRadius, whatIsProjectiles);
        HashSet<ProjectileScript> caught = new HashSet<ProjectileScript>();

        foreach (var c in hits)
        {
            ProjectileScript p = c.GetComponent<ProjectileScript>();
            if (p != null)
                caught.Add(p);
        }

        if (caught.Count == 0) return;

        foreach (var p in caught)
            Destroy(p.gameObject);

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddScore(scorePerBullet * caught.Count);

        Debug.Log($"[PlayerDeflect] Destroyed {caught.Count} bullet(s). +{scorePerBullet * caught.Count} coins");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (cam != null)
        {
            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;
            Gizmos.DrawWireSphere(mouseWorld, destroyRadius);
        }
    }
}