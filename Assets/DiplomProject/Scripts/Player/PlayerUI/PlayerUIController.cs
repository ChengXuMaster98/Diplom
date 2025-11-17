using UnityEngine;
using Zenject;

public class PlayerUIController : IInitializable, ILateTickable
{
    private readonly PlayerHealth _health;
    private readonly StaminaSystem _staminaSystem;
    private readonly HealthBar _healthBar;
    private readonly StaminaBar _staminaBar;
    private readonly LowHealthEffect _lowHealthEffect;

    private float _currentHealthRatio;
    private float _currentStaminaRatio;

    [Inject]
    public PlayerUIController(
        PlayerHealth health,
        StaminaSystem staminaSystem,
        HealthBar healthBar,
        StaminaBar staminaBar,
        LowHealthEffect lowHealthEffect)
    {
        _health = health;
        _staminaSystem = staminaSystem;
        _healthBar = healthBar;
        _staminaBar = staminaBar;
        _lowHealthEffect = lowHealthEffect;
    }

    public void Initialize()
    {
        _health.OnHealthChanged += OnHealthChanged;
        _staminaSystem.OnStaminaChanged += OnStaminaChanged;

        OnHealthChanged(_health.CurrentHealth);
        OnStaminaChanged(_staminaSystem.CurrentStamina);
    }

    private void OnHealthChanged(int currentHealth)
    {
        _currentHealthRatio = (float)currentHealth / _health.MaxHealth;
        _healthBar.SetTarget(_currentHealthRatio);
        _lowHealthEffect.UpdateEffect(_currentHealthRatio);
    }

    private void OnStaminaChanged(float currentStamina)
    {
        _currentStaminaRatio = currentStamina / _staminaSystem.MaxStamina;
        _staminaBar.SetTarget(_currentStaminaRatio);
    }

    public void LateTick()
    {
        // UI обновляется через Update в самих барах
    }
}