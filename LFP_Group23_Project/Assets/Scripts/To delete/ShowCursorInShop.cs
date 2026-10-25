using UnityEngine;

public class ShowCursorInShop : MonoBehaviour
{
    void OnEnable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void OnDisable()
    {
        // Optional: hide again when leaving the shop, if you want.
        // Comment these out if you'd rather let PlayerCombatSystem re-hide it.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
    }
}