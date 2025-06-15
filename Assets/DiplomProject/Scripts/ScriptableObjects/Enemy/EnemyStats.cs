using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "EnemyStatistics/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    public int MaxHealth;
    public int Damage;
    public float MoveSpeed;
    public float AttackRange = 1.5f;
}