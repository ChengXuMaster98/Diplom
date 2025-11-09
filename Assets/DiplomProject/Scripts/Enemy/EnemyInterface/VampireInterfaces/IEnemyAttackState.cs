using UnityEngine;

public interface IEnemyAttackState : IEnemyState
{
    void SetTarget(Transform target, IPlayerDamageable damageable);
}