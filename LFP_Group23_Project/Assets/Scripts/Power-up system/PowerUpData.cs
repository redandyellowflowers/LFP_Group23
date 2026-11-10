using UnityEngine;

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

    [Header("Effect")]
    public PowerUpType type;
    public float amount = 0.1f;
}

public enum PowerUpType
{
    MoveSpeed,       // % boost to run speed
    Armor,           // +1 armor per amount
    ScoreMultiplier, // % boost to coins collected (capped at 0.5)
}