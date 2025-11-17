using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerCharacteristics/PlayerStats", order =2)]
public class PlayerStats : ScriptableObject
{
    public int MaxHealth = 100;
    public int attackDamage = 20;
    public float MoveSpeed = 5f;
    public float mouseSensitivity = 100f;
    public float RotationSpeed = 2f;
    public float JumpHeight;
    public float gravity = -9.81f;
    public float HealthRegenRate = 3f;
}