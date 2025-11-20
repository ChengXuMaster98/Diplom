using UnityEngine;
using System;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour, IEnemy
{
    private EnemyStats _stats;
    private int _currentHealth;

    public bool IsDead { get; private set; }

    public event Action OnDeath;
    public event Action OnDamaged;


    public void Initialize(EnemyStats stats)
    {
        _stats = stats;
        _currentHealth = stats.MaxHealth;
        IsDead = false;
    }


    public void TakeDamage(int damage)
    {
        if (IsDead)
        {
            Debug.Log($"[Enemy] Уже мертв, урон не применяется");
            return;
        }

        _currentHealth -= damage;

        GetComponent<EnemySoundController>()?.PlayHurt();

        OnDamaged?.Invoke();

        Debug.Log($"[Enemy] Получен урон: {damage}, Текущий HP: {_currentHealth}");


        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (IsDead) return;

        IsDead = true;
        Debug.Log($"[Enemy] Умер! Установлено IsDead = true");

        GetComponent<EnemySoundController>()?.PlayDeath();

        OnDeath?.Invoke();
        //Destroy(gameObject);
    }

    public void ApplyStun(float stunDuration)
    {
        throw new NotImplementedException();
    }

    public void ApplyDOT(float dOTDamagePerSecond, float dOTDuration)
    {
        throw new NotImplementedException();
    }
}