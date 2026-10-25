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
        Debug.Log($"[ApplyPowerUpsOnStart] Running in scene: {gameObject.scene.name}");           
        if (PowerUpManager.Instance == null)
        {
            Debug.LogError("[ApplyPowerUpsOnStart] PowerUpManager.Instance is NULL!");                
            return;
        }
        Debug.Log($"[ApplyPowerUpsOnStart] Owned power-ups: {PowerUpManager.Instance.ownedPowerUps.Count}"); 
        PowerUpManager.Instance.ApplyToAllPlayersInScene();
    }

}