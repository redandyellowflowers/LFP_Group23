using UnityEngine;

public class PlayerCombatSystem : MonoBehaviour
{
    [Header("Aiming")]
    public bool flipSprite = true;

    [Header("Crosshair")]
    public Transform crosshair;
    public Vector3 crosshairOffset = Vector3.zero;

    private Camera cam;

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
    }

    Vector2 GetAimDirection(Vector3 mouseWorld)
    {
        Vector2 dir = (Vector2)mouseWorld - (Vector2)transform.position;
        if (dir.sqrMagnitude < 0.0001f) return Vector2.right;
        return dir.normalized;
    }
}