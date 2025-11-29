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
    private float _dotDamageAccum;

    public bool IsStunned => _stunTimer > 0f;
    public event Action<float> OnStunned; // продолжительность ивента стана

    public bool IsDead { get; private set; }

    public event Action OnDeath;
    public event Action OnDamaged;
    public event Action<float> DotApplied;

    public Vector3 CenterPoint => transform.position + Vector3.up * 1.2f;


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

        DotApplied?.Invoke(duration); // Вот здесь событие вызывается дота.

        Debug.Log($"[Enemy] DOT applied ({damagePerSecond} DPS, {duration}s)");
    }

    private void TickDoT()
    {
        if (_dotTimer <= 0f || _dotDamagePerSecond <= 0f)
            return;

        _dotTimer -= Time.deltaTime;

        // Накапливаем дробный урон
        float dmgThisFrame = _dotDamagePerSecond * Time.deltaTime;
        _dotDamageAccum += dmgThisFrame;

        int intDamage = Mathf.FloorToInt(_dotDamageAccum);
        if (intDamage > 0)
        {
            _dotDamageAccum -= intDamage;
            ApplyDamageInternal(intDamage, triggerHitReaction: false);
        }

        if (_dotTimer <= 0f)
        {
            _dotDamagePerSecond = 0f;
            _dotDamageAccum = 0f;
        }
    }

    private void ApplyDamageInternal(int damage, bool triggerHitReaction)
    {
        if (IsDead)
            return;

        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _stats.MaxHealth);

        Debug.Log($"[Enemy] Получен урон: {damage}, Текущий HP: {_currentHealth}");

        if (triggerHitReaction)
        {
            GetComponent<EnemySoundController>()?.PlayHurt();
            OnDamaged?.Invoke();
        }

        if (_currentHealth <= 0)
        {
            Die();
        }
    }


    public void TakeDamage(int damage)
    {
        if (IsDead)
        {
            Debug.Log($"[Enemy] Уже мертв, урон не применяется");
            return;
        }

        ApplyDamageInternal(damage, triggerHitReaction: true);
    }

    private void Die()
    {
        if (IsDead) return;

        IsDead = true;
        Debug.Log($"[Enemy] Умер! Установлено IsDead = true");

        GetComponent<EnemySoundController>()?.PlayDeath();

        OnDeath?.Invoke();
    }
}