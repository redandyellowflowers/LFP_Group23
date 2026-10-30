using System.Collections.Generic;
using UnityEngine;

public class PlayerDeflect : MonoBehaviour
{
    [Header("Deflect")]
    [Tooltip("Radius around the cursor where bullets get deflected.")]
    public float deflectRadius = 0.75f;

    [Tooltip("What layers count as projectiles.")]
    public LayerMask whatIsProjectiles;

    [Header("Reward")]
    public int scorePerDeflect = 25;

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

        Collider2D[] hits = Physics2D.OverlapCircleAll(mouseWorld, deflectRadius, whatIsProjectiles);
        HashSet<Projectile> caught = new HashSet<Projectile>();

        foreach (var c in hits)
        {
            Projectile p = c.GetComponent<Projectile>();
            if (p != null && !p.IsDeflected)
                caught.Add(p);
        }

        if (caught.Count == 0) return;

        // Deflect direction = away from the player, toward the cursor
        Vector2 dir = ((Vector2)mouseWorld - (Vector2)transform.position).normalized;
        if (dir.sqrMagnitude < 0.0001f) dir = Vector2.right;

        foreach (var p in caught)
            p.Deflect(dir);

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddScore(scorePerDeflect);

        Debug.Log($"[PlayerRightClickDeflect] Deflected {caught.Count} bullet(s). +{scorePerDeflect} coins");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (cam != null)
        {
            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;
            Gizmos.DrawWireSphere(mouseWorld, deflectRadius);
        }
    }
}