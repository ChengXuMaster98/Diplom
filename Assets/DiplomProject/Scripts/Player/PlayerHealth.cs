using UnityEngine;
using Zenject;
using System;

public class PlayerHealth : MonoBehaviour, IInitializable, IPlayerDamageable
{
    public event Action OnDeath;
    public event Action<int> OnHealthChanged;


    private int _currentHealth;
    private PlayerStats _stats;
    private GameOverUI _gameOverUI;

    private CharacterMovementController _movementController;
    public bool IsDead => _currentHealth <= 0;

    [Inject]
    public void Construct(PlayerStats stats, GameOverUI gameOverUI, CharacterMovementController MovementController)
    {
        Debug.Log("[PlayerHealth] Injected stats.MaxHealth = " + stats.MaxHealth);
        _stats = stats;
        _gameOverUI = gameOverUI;
        _movementController = MovementController;
        // Сделать здесь максимальное здоровье
        // Сделать PlayerController, он будет иметь зависимость на Player, прописать там логику отнимания здоровья
    }

    public void Initialize()
    {
        Debug.Log("[PlayerHealth] Initialize called!");
        _currentHealth = _stats.MaxHealth;
        OnHealthChanged?.Invoke(_currentHealth);
    }

    public void TakeDamage(int amount)
    {
        Debug.Log($"[PlayerHealth] TakeDamage called, amount={amount}, current={_currentHealth}");
        _currentHealth -= amount;
        _currentHealth = Mathf.Max(_currentHealth, 0);
        Debug.Log($"[PlayerHealth] After damage current={_currentHealth}");
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

        _movementController.BlockMovement();

        // Запускаем событие смерти
        OnDeath?.Invoke();

        _gameOverUI.ShowGameOverScreen();

    }
}