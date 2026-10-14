using UnityEngine;

public class ApplyPowerUpsOnStart : MonoBehaviour
{
   /* void Start()
    {
        if (PowerUpManager.Instance != null)
            PowerUpManager.Instance.ApplyToAllPlayersInScene();
    }*/
    void Start()
    {
        Debug.Log($"[ApplyPowerUpsOnStart] Running in scene: {gameObject.scene.name}");               // NEW
        if (PowerUpManager.Instance == null)
        {
            Debug.LogError("[ApplyPowerUpsOnStart] PowerUpManager.Instance is NULL!");                // NEW
            return;
        }
        Debug.Log($"[ApplyPowerUpsOnStart] Owned power-ups: {PowerUpManager.Instance.ownedPowerUps.Count}"); // NEW
        PowerUpManager.Instance.ApplyToAllPlayersInScene();
    }

}