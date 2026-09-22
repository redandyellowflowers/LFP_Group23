using TMPro;
using UnityEngine;

public class PlayerCombatSystem : MonoBehaviour
{
    [Header("Light Attack")]
    public float startTimeBtwLightAttack = 0.3f;
    public int lightDamage = 10;
    public float lightAttackRange = 1.0f;

    [Header("Heavy Attack")]
    public float startTimeBtwHeavyAttack = 0.8f;
    public int heavyDamage = 25;
    public float heavyAttackRange = 1.5f;

    [Header("Aiming")]
    public float attackDistance = 0.8f;   // how far in front of the player the sword hitbox sits
    public bool flipSprite = true;        // flip the player to face the cursor horizontally

    [Header("Crosshair")]
    public Transform crosshair;           
    public Vector3 crosshairOffset = Vector3.zero;
    public bool hideSystemCursor = true;  // hide the OS cursor while playing

    [Header("References")]
    public LayerMask whatIsEnemies;
    public LayerMask whatIsProjectiles;

    private float lightAttackTimer;
    private float heavyAttackTimer;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        if (hideSystemCursor)
            Cursor.visible = false;
    }

    void OnDisable()
    {
        // Restore the system cursor if this script is disabled/destroyed
        if (hideSystemCursor)
            Cursor.visible = true;
    }

    void Update()
    {
        // ---- Aim toward the cursor ----
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // Move the crosshair to the cursor position
        if (crosshair != null)
            crosshair.position = mouseWorld + crosshairOffset;

        Vector2 aimDirection = GetAimDirection(mouseWorld);
        Vector2 attackPoint = (Vector2)transform.position + aimDirection * attackDistance;

        // Flip the player sprite to face the cursor horizontally
        if (flipSprite && Mathf.Abs(aimDirection.x) > 0.01f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(aimDirection.x);
            transform.localScale = scale;
        }

        // ---- Light Attack (Left Mouse Button) ----
        if (lightAttackTimer <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(
                    attackPoint, lightAttackRange, whatIsEnemies);

                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    Enemy e = enemiesToDamage[i].GetComponent<Enemy>();
                    if (e != null) e.TakeDamage(lightDamage, false);
                }

                Debug.Log("Light attack toward cursor!");
                lightAttackTimer = startTimeBtwLightAttack;
            }
        }
        else
        {
            lightAttackTimer -= Time.deltaTime;
        }

        // ---- Heavy Attack (Right Mouse Button) ----
        if (heavyAttackTimer <= 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(
                    attackPoint, heavyAttackRange, whatIsEnemies);

                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    Enemy e = enemiesToDamage[i].GetComponent<Enemy>();
                    if (e != null) e.TakeDamage(heavyDamage, true);
                }

                Collider2D[] projectilesToDeflect = Physics2D.OverlapCircleAll(
                    attackPoint, heavyAttackRange, whatIsProjectiles);

                for (int i = 0; i < projectilesToDeflect.Length; i++)
                {
                    Projectile p = projectilesToDeflect[i].GetComponent<Projectile>();
                    if (p != null) p.Deflect();
                }

                Debug.Log("Heavy attack toward cursor!");
                heavyAttackTimer = startTimeBtwHeavyAttack;
            }
        }
        else
        {
            heavyAttackTimer -= Time.deltaTime;
        }
    }

    Vector2 GetAimDirection(Vector3 mouseWorld)
    {
        Vector2 dir = (Vector2)mouseWorld - (Vector2)transform.position;
        if (dir.sqrMagnitude < 0.0001f) return Vector2.right;
        return dir.normalized;
    }

    void OnDrawGizmosSelected()
    {
        Vector2 previewDir = Vector2.right;
        Vector2 previewPoint = (Vector2)transform.position + previewDir * attackDistance;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(previewPoint, lightAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(previewPoint, heavyAttackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, previewPoint);
    }
}