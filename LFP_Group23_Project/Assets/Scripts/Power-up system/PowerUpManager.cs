using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    [Header("All available power-ups in the game")]
    public List<PowerUpData> allPowerUps = new List<PowerUpData>();

    [Header("Currently owned (runtime)")]
    public List<PowerUpData> ownedPowerUps = new List<PowerUpData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
   
    public bool HasPowerUp(PowerUpData data) => ownedPowerUps.Contains(data);

    public void AddPowerUp(PowerUpData data)
    {
        if (data == null) return;
        ownedPowerUps.Add(data);
        Debug.Log($"[PowerUpManager] Added '{data.powerUpName}' type={data.type} amount={data.amount}"); // NEW
        ApplyToAllPlayersInScene();
    }

    public void ClearPowerUps() => ownedPowerUps.Clear();

    // Returns the combined multiplier for a given type. E.g. if the player has
    // two "+10% damage" power-ups, this returns 1.20.
    public float GetMultiplier(PowerUpType type)
    {
        float total = 0f;
        foreach (var p in ownedPowerUps)
            if (p.type == type) total += p.amount;
        return 1f + total;
    }

    // Called by GameBootstrap (see below) at level start to apply effects.
    public void ApplyToAllPlayersInScene()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;

        // Move speed
        var cc = player.GetComponent<CharacterControllerScript_Miguel>();
        if (cc != null)
            cc.runSpeed = 40f * GetMultiplier(PowerUpType.MoveSpeed);

        // Combat
        var combat = player.GetComponent<PlayerCombatSystem>();
        if (combat != null)
        {
            combat.damage = Mathf.RoundToInt(10 * GetMultiplier(PowerUpType.Damage));
            combat.attackCooldown = 0.3f / GetMultiplier(PowerUpType.AttackCooldown);
            combat.bladeRadius = 0.2f * GetMultiplier(PowerUpType.BladeRadius);
        }

        // Armor — each Armor power-up grants +1 armor (amount treated as a flat count)
        var health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            int totalArmor = 0;
            foreach (var p in ownedPowerUps)
                if (p.type == PowerUpType.Armor)
                    totalArmor += Mathf.RoundToInt(p.amount);  // 1 = +1 armor
            health.armor = totalArmor;
        }
    }
}