using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatSystem : MonoBehaviour
{
    [Header("Attack")]
    public float attackCooldown = 0.3f;
    public int damage = 10;

    [Header("Swipe Settings")]
    public float minSwipeDistance = 0.0f;
    public float maxSwipeDuration = 0.6f;
    public int swipeSamplePoints = 12;
    public float bladeRadius = 0.2f;

    [Header("Aiming")]
    public bool flipSprite = true;

    [Header("Crosshair")]
    public Transform crosshair;
    public Vector3 crosshairOffset = Vector3.zero;

    [Header("References")]
    public LayerMask whatIsEnemies;
    public LayerMask whatIsProjectiles;

    private float attackTimer;
    private Camera cam;

    private bool isSwiping = false;
    private Vector2 swipeStartWorld;
    private float swipeStartTime;
    private List<Vector2> swipePoints = new List<Vector2>();

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
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

        if (attackTimer > 0) attackTimer -= Time.deltaTime;

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

        if (duration > maxSwipeDuration || distance < minSwipeDistance)
            return;

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