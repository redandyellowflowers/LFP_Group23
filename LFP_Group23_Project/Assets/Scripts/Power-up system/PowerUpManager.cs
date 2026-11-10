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
            return;
        }
    }

    public bool HasPowerUp(PowerUpData data) => ownedPowerUps.Contains(data);

    public void AddPowerUp(PowerUpData data)
    {
        if (data == null) return;
        ownedPowerUps.Add(data);
        Debug.Log($"[PowerUpManager] Added '{data.powerUpName}' type={data.type} amount={data.amount}"); 
        ApplyToAllPlayersInScene();
    }

    public void ClearPowerUps() => ownedPowerUps.Clear();

    public float GetMultiplier(PowerUpType type)
    {
        float total = 0f;
        foreach (var p in ownedPowerUps)
            if (p.type == type) total += p.amount;

        // Cap coin multiplier bonus at +50%
        if (type == PowerUpType.ScoreMultiplier)
            total = Mathf.Min(total, 0.5f);

        return 1f + total;
    }
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
            /*combat.damage = Mathf.RoundToInt(10 * GetMultiplier(PowerUpType.Damage));
            combat.attackCooldown = 0.3f / GetMultiplier(PowerUpType.AttackCooldown);
            combat.bladeRadius = 0.2f * GetMultiplier(PowerUpType.BladeRadius);*/
        }

        // Armor — each Armor power-up grants +1 armor (amount treated as a flat count)
        var health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            int totalArmor = 0;
            foreach (var p in ownedPowerUps)
                if (p.type == PowerUpType.Armor)
                    totalArmor += Mathf.RoundToInt(p.amount);  
            health.armor = totalArmor;
        }
    }
}