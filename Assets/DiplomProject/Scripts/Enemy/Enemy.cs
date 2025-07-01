using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IEnemy
{
    private EnemyStats _stats;
    private int _currentHealth;

    public bool IsDead { get; private set; }

    public event Action OnDeath;

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
            Debug.Log($"[Enemy] ��� ����, ���� �� �����������");
            return;
        }

        _currentHealth -= damage;
        Debug.Log($"[Enemy] ������� ����: {damage}, ������� HP: {_currentHealth}");
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (IsDead) return;

        IsDead = true;
        Debug.Log($"[Enemy] ����! ������������� IsDead = true");

        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}