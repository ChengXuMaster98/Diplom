using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AttackHitBox : MonoBehaviour
{
    private PlayerStats _playerStats;
    private bool _canHit = false;

    [Inject]
    public void Construct(PlayerStats stats)
    {
        _playerStats = stats;
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
            enemy.TakeDamage(_playerStats.attackDamage);
            _canHit = false;
            Debug.Log($"[AttackHitBox] ”рон по врагу:");
        }
    }
}