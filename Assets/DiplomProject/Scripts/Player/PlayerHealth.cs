using UnityEngine;
using Zenject;
using System;

public class PlayerHealth : IInitializable, IPlayerDamageable
{
    public event Action OnDeath;
    public event Action<int> OnHealthChanged;

    private int _currentHealth;
    private readonly PlayerStats _stats;
    private readonly PlayerStateSaver _stateSaver;
    private readonly GameOverUI _gameOverUI;

    [Inject]
    public PlayerHealth(PlayerStats stats, PlayerStateSaver stateSaver)
    {
        _stats = stats;
        _stateSaver = stateSaver;
        // Сделать здесь максимальное здоровье
        // Сделать PlayerController, он будет иметь зависимость на Player, прописать там логику отнимания здоровья
    }

    public void Initialize()
    {
        _currentHealth = _stateSaver != null ? _stateSaver._currentHealth : _stats.MaxHealth;
        OnHealthChanged?.Invoke(_currentHealth);
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Max(_currentHealth, 0);
        OnHealthChanged?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Min(_currentHealth, _stats.MaxHealth);
        OnHealthChanged?.Invoke(_currentHealth);
    }

    private void Die()
    {
        Debug.Log("Player died");

        // Вызываем экран GameOver
        _gameOverUI.ShowGameOverScreen();

        // Запускаем событие смерти
        OnDeath?.Invoke();
    }
}