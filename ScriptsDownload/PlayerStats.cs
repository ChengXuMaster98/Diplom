using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float MaxHealth = 100f;
    public float Damage = 20f;
    public float MoveSpeed = 5f;
    public float AttackCooldown = 1f;
}