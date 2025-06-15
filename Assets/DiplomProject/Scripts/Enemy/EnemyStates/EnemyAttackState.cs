using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private readonly IEnemyAnimator _animator;
    private readonly IPlayerDamageable _playerDamageable;
    private readonly float _damage;

    private Transform _target;

    public EnemyAttackState(IPlayerDamageable playerDamageable, IEnemyAnimator animator, float damage)
    {
        _playerDamageable = playerDamageable;
        _damage = damage;
        _animator = animator;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }


    public void Enter()
    {
        _animator.PlayAttack();

        if (_target != null)
        {
            Vector3 direction = (_target.position - _animator.Transform.position).normalized;
            _animator.LookAt(_target.position);
        }
        
        _playerDamageable.TakeDamage((int)_damage);
    }

    public void Tick() { }

    public void Exit() { }
}