using UnityEngine;

public class VampireAttackState : IEnemyState
{
    private readonly IEnemyAnimator _animator;
    private readonly IPlayerDamageable _playerDamageable;
    private readonly EnemyStats _stats;
    private readonly IPlayerDetector _detector;


    private Transform _targetPlayer;

    public VampireAttackState(IPlayerDamageable playerDamageable, IEnemyAnimator animator, IPlayerDetector detector, EnemyStats stats)
    {
        _playerDamageable = playerDamageable;
        _stats = stats;
        _animator = animator;
        _detector = detector;
    }

    public void SetTarget(Transform target)
    {
        _targetPlayer = target;
    }


    public void Enter()
    {
        _animator.PlayAttack();

        if (_targetPlayer != null)
        {
            Vector3 direction = (_targetPlayer.position - _animator.Transform.position).normalized;
            _animator.LookAt(_targetPlayer.position);
        }

        _playerDamageable.TakeDamage(_stats.Damage);
    }

    public void Tick() { }

    public void Exit() { }
}