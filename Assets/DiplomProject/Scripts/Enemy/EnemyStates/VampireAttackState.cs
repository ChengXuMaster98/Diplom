using UnityEngine;

public class VampireAttackState : IEnemyState
{
    private readonly IEnemyAnimator _animator;
    private readonly IPlayerDamageable _playerDamageable;
    private readonly EnemyStats _stats;
    private readonly IPlayerDetector _detector;


    private Transform _target;

    public VampireAttackState(IPlayerDamageable playerDamageable, IEnemyAnimator animator, IPlayerDetector detector, EnemyStats stats)
    {
        _playerDamageable = playerDamageable;
        _stats = stats;
        _animator = animator;
        _detector = detector;
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

        _playerDamageable.TakeDamage(_stats.Damage);
    }

    public void Tick() { }

    public void Exit() { }
}