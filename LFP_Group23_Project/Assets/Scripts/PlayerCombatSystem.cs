using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCombatSystem : MonoBehaviour
{
    [Header("Light Attack")]
    public float startTimeBtwLightAttack = 0.3f;
    public int lightDamage = 10;
    public float lightAttackRange = 1.0f;   // kept for gizmo preview only

    [Header("Heavy Attack")]
    public float startTimeBtwHeavyAttack = 0.8f;
    public int heavyDamage = 25;
    public float heavyAttackRange = 1.5f;   // kept for gizmo preview only

    [Header("Swipe Settings")]
    //Minimum distance (world units) the mouse must travel for a swipe to count.
    public float minSwipeDistance = 0.8f;

    //Maximum time (seconds) from press to release for a swipe to count
    public float maxSwipeDuration = 0.4f;

    //How many points to sample along the swipe line. Higher = better on fast swipes.
    public int swipeSamplePoints = 12;

    //Radius of the blade hitbox at each sample point.
    public float lightBladeRadius = 0.15f;
    public float heavyBladeRadius = 0.25f;

    [Header("Aiming")]
    public float attackDistance = 0.8f;   // kept for gizmo preview only
    public bool flipSprite = true;

    [Header("Crosshair")]
    public Transform crosshair;
    public Vector3 crosshairOffset = Vector3.zero;
    public bool hideSystemCursor = true;

    [Header("References")]
    public LayerMask whatIsEnemies;
    public LayerMask whatIsProjectiles;

   /* [Header("Swipe Trail (optional)")]
    public LineRenderer swipeTrail;*/

    private float lightAttackTimer;
    private float heavyAttackTimer;
    private Camera cam;

    // Swipe state
    private bool isSwiping = false;
    private bool swipeIsHeavy = false;
    private Vector2 swipeStartWorld;
    private float swipeStartTime;
    private List<Vector2> swipePoints = new List<Vector2>();

    void Start()
    {
        cam = Camera.main;

        if (hideSystemCursor)
            Cursor.visible = false;

       /* if (swipeTrail != null)
            swipeTrail.positionCount = 0;*/
    }

  /*  void OnDisable()
    {
        if (hideSystemCursor && ! pauseMenu.IsPaused)
            Cursor.visible = true;
    }*/

    void Update()
    {
       // if (PauseMenu.IsPaused) return;

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

        // ---- Tick cooldowns ----
        if (lightAttackTimer > 0) lightAttackTimer -= Time.deltaTime;
        if (heavyAttackTimer > 0) heavyAttackTimer -= Time.deltaTime;

        // ---- Swipe lifecycle ----
        if (!isSwiping)
        {
            if (Input.GetMouseButtonDown(0) && lightAttackTimer <= 0)
                BeginSwipe(mouseWorld, false);
            else if (Input.GetMouseButtonDown(1) && heavyAttackTimer <= 0)
                BeginSwipe(mouseWorld, true);
        }
        else
        {
            bool stillHolding = swipeIsHeavy
                ? Input.GetMouseButton(1)
                : Input.GetMouseButton(0);

            if (stillHolding)
            {
                swipePoints.Add(mouseWorld);
               // UpdateTrail();
            }
            else
            {
                EndSwipe(mouseWorld);
            }
        }
    }

    void BeginSwipe(Vector2 worldPos, bool isHeavy)
    {
        isSwiping = true;
        swipeIsHeavy = isHeavy;
        swipeStartWorld = worldPos;
        swipeStartTime = Time.time;
        swipePoints.Clear();
        swipePoints.Add(worldPos);

      /*  if (swipeTrail != null)
        {
            swipeTrail.positionCount = 0;
            swipeTrail.startColor = isHeavy
                ? new Color(0.3f, 0.5f, 1f, 1f)   // blue for heavy
                : new Color(1f, 1f, 1f, 1f);      // white for light
            swipeTrail.endColor = swipeTrail.startColor;
        }*/
    }

    void EndSwipe(Vector2 worldEnd)
    {
        isSwiping = false;
        swipePoints.Add(worldEnd);

        float duration = Time.time - swipeStartTime;
        float distance = Vector2.Distance(swipeStartWorld, worldEnd);

        // Reject weak / slow swipes
        if (duration > maxSwipeDuration || distance < minSwipeDistance)
        {
           // ClearTrail();
            return;
        }

        PerformSwipeHit(swipeStartWorld, worldEnd, swipeIsHeavy);

        if (swipeIsHeavy)
        {
            heavyAttackTimer = startTimeBtwHeavyAttack;
            Debug.Log("Heavy swipe!");
        }
        else
        {
            lightAttackTimer = startTimeBtwLightAttack;
            Debug.Log("Light swipe!");
        }

       // ClearTrail();
    }

    void PerformSwipeHit(Vector2 start, Vector2 end, bool isHeavy)
    {
        int damage = isHeavy ? heavyDamage : lightDamage;
        float radius = isHeavy ? heavyBladeRadius : lightBladeRadius;

        HashSet<Enemy> hitEnemies = new HashSet<Enemy>();
        HashSet<Projectile> hitProjectiles = new HashSet<Projectile>();

        // Walk along the swipe line and sample circles
        for (int i = 0; i <= swipeSamplePoints; i++)
        {
            float t = i / (float)swipeSamplePoints;
            Vector2 samplePoint = Vector2.Lerp(start, end, t);

            Collider2D[] enemyHits = Physics2D.OverlapCircleAll(samplePoint, radius, whatIsEnemies);
            foreach (var c in enemyHits)
            {
                Enemy e = c.GetComponent<Enemy>();
                if (e != null) hitEnemies.Add(e);
            }

            if (isHeavy)
            {
                Collider2D[] projHits = Physics2D.OverlapCircleAll(samplePoint, radius, whatIsProjectiles);
                foreach (var c in projHits)
                {
                    Projectile p = c.GetComponent<Projectile>();
                    if (p != null) hitProjectiles.Add(p);
                }
            }
        }

        foreach (var e in hitEnemies)
            e.TakeDamage(damage, isHeavy);

        foreach (var p in hitProjectiles)
            p.Deflect();
    }
/*
    void UpdateTrail()
    {
        if (swipeTrail == null || swipePoints.Count < 2) return;

        swipeTrail.positionCount = swipePoints.Count;
        for (int i = 0; i < swipePoints.Count; i++)
            swipeTrail.SetPosition(i, swipePoints[i]);
    }

    void ClearTrail()
    {
        swipePoints.Clear();
        if (swipeTrail != null)
            swipeTrail.positionCount = 0;
    }
*/
    Vector2 GetAimDirection(Vector3 mouseWorld)
    {
        Vector2 dir = (Vector2)mouseWorld - (Vector2)transform.position;
        if (dir.sqrMagnitude < 0.0001f) return Vector2.right;
        return dir.normalized;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, lightBladeRadius);

        Gizmos.color = new Color(0.3f, 0.5f, 1f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, heavyBladeRadius);
    }
}