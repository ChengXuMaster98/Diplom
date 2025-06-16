using UnityEngine;
using System;
using Zenject;

public class Enemy : MonoBehaviour, IEnemy
{
    private EnemyStats _stats;
    private int _currentHealth;
    private IEnemyAnimator _enemyAnimator;

    public bool IsDead { get; private set; }

    public event Action OnDeath;


    [Inject]
    public void Construct(IEnemyAnimator enemyAnimator)
    {
        _enemyAnimator = enemyAnimator;
    }
    private void Start()
    {
        // Инициализация AnimatorController (на этом этапе он точно применён)
        Debug.Log($"[Enemy] Вражеский аниматор инициализирован");
        _enemyAnimator?.Initialize();
    }
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
            Debug.Log($"[Enemy] Уже мёртв, урон не применяется");
            return;
        }

        _currentHealth -= damage;
        Debug.Log($"[Enemy] Получен урон: {damage}, текущее HP: {_currentHealth}");
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (IsDead) return;

        IsDead = true;
        Debug.Log($"[Enemy] Умер! Устанавливаем IsDead = true");

        _enemyAnimator?.PlayDie();

        OnDeath?.Invoke();
        Destroy(gameObject, 2f);

}
}