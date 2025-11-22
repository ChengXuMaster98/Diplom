using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IEnemy, IStunnable, IDamageOverTime
{
    private EnemyStats _stats;
    private int _currentHealth;

    //WeaponSkills
    private float _stunTimer = 0f;

    private float _dotTimer = 0f;
    private float _dotDamagePerSecond = 0f;

    public bool IsStunned => _stunTimer > 0f;
    public event Action<float> OnStunned; // продолжительность ивента стана

    public bool IsDead { get; private set; }

    public event Action OnDeath;
    public event Action OnDamaged;



    public void Initialize(EnemyStats stats)
    {
        _stats = stats;
        _currentHealth = stats.MaxHealth;
        IsDead = false;
    }

    private void Update()
    {
        TickStun();
        TickDoT();
    }


    // ----------- STUN -----------
    public void ApplyStun(float duration)
    {
        if (IsDead) return;

        // Если уже оглушён, то обновляем ТОЛЬКО если новое время дольше
        if (_stunTimer > 0f)
        {
            if (duration > _stunTimer)
            {
                _stunTimer = duration;
                OnStunned?.Invoke(duration); // обновленный стан
                Debug.Log($"[Enemy] Stun refreshed to {duration}s");
            }

            return; // Если новый стан слабее — ничего не делаем
        }

        // Если НЕ был оглушён — просто ставим таймер
        _stunTimer = duration;
        OnStunned?.Invoke(duration);
        Debug.Log($"[Enemy] Stunned for {duration}s");
    }

    private void TickStun()
    {
        if (_stunTimer <= 0f) return;

        _stunTimer -= Time.deltaTime;

        if (_stunTimer <= 0f)
        {
            Debug.Log("[Enemy] Stun ended");
        }
    }

    // ----------- DOT -----------
    public void ApplyDoT(float damagePerSecond, float duration)
    {
        if (IsDead) return;

        _dotDamagePerSecond = damagePerSecond;
        _dotTimer = duration;

        Debug.Log($"[Enemy] DOT applied ({damagePerSecond} DPS, {duration}s)");
    }

    private void TickDoT()
    {
        if (_dotTimer <= 0f) return;

        _dotTimer -= Time.deltaTime;
        TakeDamage(Mathf.RoundToInt(_dotDamagePerSecond * Time.deltaTime));
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
}