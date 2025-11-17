using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AttackHitBox : MonoBehaviour
{
    private PlayerStats _playerStats;
    private bool _canHit = false;

    private IUpgradeService _upgradeService;

    [Inject]
    public void Construct(PlayerStats stats, IUpgradeService upgradeService)
    {
        _playerStats = stats;
        _upgradeService = upgradeService;
    }

    public void EnableHitbox()
    {
        _canHit = true;
        gameObject.SetActive(true);
    }

    public void DisableHitbox()
    {
        _canHit = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_canHit)
            return;
        
        if (other.TryGetComponent<IEnemy>(out var enemy))
        {
            int damage = Mathf.RoundToInt(_playerStats.attackDamage * _upgradeService.DamageMultiplier);
            enemy.TakeDamage(damage);
            _canHit = false;
            Debug.Log($"[AttackHitBox] ”рон по врагу:");
        }
    }
}