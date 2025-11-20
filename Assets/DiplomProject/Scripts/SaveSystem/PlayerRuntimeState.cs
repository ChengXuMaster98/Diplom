using System.Collections.Generic;
using UnityEngine;

public class PlayerRuntimeState
{
    public Vector3 Position;
    public int MaxHealth;
    public int CurrentHealth;
    public int AttackDamage;
    public float MoveSpeed;
    public float Stamina;

    public readonly List<string> Upgrades = new();

    public PlayerRuntimeState(PlayerStats baseStats)
    {
        MaxHealth = baseStats.MaxHealth;
        CurrentHealth = baseStats.MaxHealth;
        AttackDamage = baseStats.attackDamage;
        MoveSpeed = baseStats.MoveSpeed;
        Stamina = 100f; // пример
    }
}