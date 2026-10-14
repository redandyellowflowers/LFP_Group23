using UnityEngine;

public enum PowerUpRarity { Common, Rare, Epic, Legendary }

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "PowerUps/PowerUp")]
public class PowerUpData : ScriptableObject
{
    [Header("Identity")]
    public string powerUpName = "New Power-Up";
    [TextArea(2, 4)]
    public string description = "Does something cool.";
    public Sprite icon;

    [Header("Economy")]
    public int cost = 100;
    public PowerUpRarity rarity = PowerUpRarity.Common;

    [Header("Effect")]
    public PowerUpType type;
    public float amount = 0.1f;

    public Color RarityColor
    {
        get
        {
            switch (rarity)
            {
                case PowerUpRarity.Common: return new Color(0.8f, 0.8f, 0.8f);
                case PowerUpRarity.Rare: return new Color(0.3f, 0.6f, 1f);
                case PowerUpRarity.Epic: return new Color(0.7f, 0.3f, 1f);
                case PowerUpRarity.Legendary: return new Color(1f, 0.7f, 0.1f);
            }
            return Color.white;
        }
    }
}

public enum PowerUpType
{
    MoveSpeed,       // % boost to run speed
    Damage,          // % boost to attack damage
    AttackCooldown,  // % reduction to attack cooldown
    BladeRadius,     // % boost to blade hit radius
    Armor,           // +1 armor per amount (see PlayerHealth)
    ScoreMultiplier, // reserved for later
    MaxHealth,       // reserved for later
}