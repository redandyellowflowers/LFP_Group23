using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatSystem : MonoBehaviour
{
    [Header("Attack")]
    public float attackCooldown = 0.3f;
    public int damage = 10;

    [Header("Swipe Settings")]
    // Minimum distance (world units) the mouse must travel for a swipe to count.
    public float minSwipeDistance = 0.0f;   // CHANGED: 0 = any distance works

    // Maximum time (seconds) from press to release for a swipe to count
    public float maxSwipeDuration = 0.6f;  

    // How many points to sample along the swipe line.
    public int swipeSamplePoints = 12;

    // Radius of the blade hitbox at each sample point.
    public float bladeRadius = 0.2f;

    [Header("Aiming")]
    public bool flipSprite = true;

    [Header("Crosshair")]
    public Transform crosshair;
    public Vector3 crosshairOffset = Vector3.zero;
    public bool hideSystemCursor = true;

    [Header("References")]
    public LayerMask whatIsEnemies;
    public LayerMask whatIsProjectiles;

    private float attackTimer;
    private Camera cam;

    // Swipe state
    private bool isSwiping = false;
    private Vector2 swipeStartWorld;
    private float swipeStartTime;
    private List<Vector2> swipePoints = new List<Vector2>();

    void Start()
    {
        cam = Camera.main;
        if (hideSystemCursor) Cursor.visible = false;
    }

    void Update()
    {
        // ---- Aim toward the cursor ----
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        if (crosshair != null)
            crosshair.position = mouseWorld + crosshairOffset;

        Vector2 aimDirection = GetAimDirection(mouseWorld);
        if (flipSprite && Mathf.Abs(aimDirection.x) > 0.01f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(aimDirection.x);
            transform.localScale = scale;
        }

        // ---- Tick cooldown ----
        if (attackTimer > 0) attackTimer -= Time.deltaTime;

        // ---- Swipe lifecycle ----
        if (!isSwiping)
        {
            if (Input.GetMouseButtonDown(0) && attackTimer <= 0)
                BeginSwipe(mouseWorld);
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                swipePoints.Add(mouseWorld);
            }
            else
            {
                EndSwipe(mouseWorld);
            }
        }
    }

    void BeginSwipe(Vector2 worldPos)
    {
        isSwiping = true;
        swipeStartWorld = worldPos;
        swipeStartTime = Time.time;
        swipePoints.Clear();
        swipePoints.Add(worldPos);
    }

    void EndSwipe(Vector2 worldEnd)
    {
        isSwiping = false;
        swipePoints.Add(worldEnd);

        float duration = Time.time - swipeStartTime;
        float distance = Vector2.Distance(swipeStartWorld, worldEnd);

        // CHANGED: min distance is now 0, so any click counts as a slice.
        if (duration > maxSwipeDuration || distance < minSwipeDistance)
            return;

        // If the player just clicked (no drag), slice from player position to cursor.
        Vector2 start = (distance < 0.01f) ? (Vector2)transform.position : swipeStartWorld;

        PerformSwipeHit(start, worldEnd);
        attackTimer = attackCooldown;
    }

    void PerformSwipeHit(Vector2 start, Vector2 end)
    {
        HashSet<Enemy> hitEnemies = new HashSet<Enemy>();
        HashSet<Projectile> hitProjectiles = new HashSet<Projectile>();

        for (int i = 0; i <= swipeSamplePoints; i++)
        {
            float t = i / (float)swipeSamplePoints;
            Vector2 samplePoint = Vector2.Lerp(start, end, t);

            Collider2D[] enemyHits = Physics2D.OverlapCircleAll(samplePoint, bladeRadius, whatIsEnemies);
            foreach (var c in enemyHits)
            {
                Enemy e = c.GetComponent<Enemy>();
                if (e != null) hitEnemies.Add(e);
            }

            // CHANGED: projectiles are always sliceable now (no isHeavy check)
            Collider2D[] projHits = Physics2D.OverlapCircleAll(samplePoint, bladeRadius, whatIsProjectiles);
            foreach (var c in projHits)
            {
                Projectile p = c.GetComponent<Projectile>();
                if (p != null) hitProjectiles.Add(p);
            }
        }

        foreach (var e in hitEnemies) e.TakeDamage(damage, false);
        foreach (var p in hitProjectiles) p.Deflect();
    }

    Vector2 GetAimDirection(Vector3 mouseWorld)
    {
        Vector2 dir = (Vector2)mouseWorld - (Vector2)transform.position;
        if (dir.sqrMagnitude < 0.0001f) return Vector2.right;
        return dir.normalized;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, bladeRadius);
    }
}