using UnityEngine;
using Zenject;
using System;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IInitializable, IPlayerDamageable, ITickable
{
    public event Action OnDeath;
    public event Action<int> OnHealthChanged;
    public event Action<DamageData> OnBeforeTakeDamage;


    private int _currentHealth;
    private PlayerStats _stats;
    private GameOverUI _gameOverUI;

    public int CurrentHealth => _currentHealth;
    //public int MaxHealth => _stats.MaxHealth;

    private CharacterMovementController _movementController;

    private float _regenTimer;

    private IBlockStatusProvider _blockStatus;
    private IPlayerStaminaConsumer _staminaConsumer;
    public bool IsDead => _currentHealth <= 0;

    private IUpgradeService _upgradeService;

    public int MaxHealth => Mathf.RoundToInt(_stats.MaxHealth * _upgradeService.HealthMultiplier);



    [Inject]
    public void Construct(PlayerStats stats, GameOverUI gameOverUI, CharacterMovementController MovementController, IBlockStatusProvider blockStatus, IPlayerStaminaConsumer staminaConsumer, IUpgradeService upgradeService)
    {
        Debug.Log("[PlayerHealth] Injected stats.MaxHealth = " + stats.MaxHealth);
        _stats = stats;
        _gameOverUI = gameOverUI;
        _movementController = MovementController;
        _blockStatus = blockStatus;
        _staminaConsumer = staminaConsumer;
        _upgradeService = upgradeService;
    }


    public void Initialize()
    {
        Debug.Log("[PlayerHealth] Initialize called!");
        _currentHealth = MaxHealth;
        OnHealthChanged?.Invoke(_currentHealth);
    }

    public void Tick()
    {
        if (IsDead || _currentHealth >= MaxHealth)
            return;

        _regenTimer += Time.deltaTime;

        // Через 3 секунды после последнего урона запускаем постепенную регенерацию
        if (_regenTimer >= 3f)
        {
            float regenPerSecond = _stats.HealthRegenRate * _upgradeService.HealthMultiplier;
            _currentHealth += Mathf.CeilToInt(regenPerSecond * Time.deltaTime);
            _currentHealth = Mathf.Min(_currentHealth, MaxHealth);
            OnHealthChanged?.Invoke(_currentHealth);
        }
    }

    public void TakeDamage(int amount)
    {


        if (_blockStatus.IsBlocking && _staminaConsumer.CanBlock())
        {
            _staminaConsumer.ConsumeStaminaForBlock();
            Debug.Log("[Block] Damage blocked!");
            return;
        }

        Debug.Log($"[PlayerHealth] TakeDamage called, amount={amount}, current={_currentHealth}");


        _currentHealth -= amount;
        

        _currentHealth = Mathf.Max(_currentHealth, 0);
        Debug.Log($"[PlayerHealth] After damage current={_currentHealth}");
                
        OnHealthChanged?.Invoke(_currentHealth);

        _regenTimer = 0f;

        if (_currentHealth <= 0)
        {
            StartCoroutine(WaitUntilHealthBarEmptiesThenDie());
        }
    }

    public IEnumerator WaitUntilHealthBarEmptiesThenDie()
    {
        // Небольшая пауза, пока UI обновляется
        yield return new WaitForSeconds(0.25f);
        Die();
    }

    public void Heal(int amount)
    {

        _currentHealth += amount;
        _currentHealth = Mathf.Min(_currentHealth, MaxHealth);
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