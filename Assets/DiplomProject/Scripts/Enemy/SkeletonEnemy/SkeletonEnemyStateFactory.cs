using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class SkeletonStateFactory : ISkeletonStateFactory
{

    private readonly ISkeletonStateMachine _stateMachine;
    private readonly IPlayerDetector _detector;
    private readonly EnemyStats _stats;
    private readonly ISkeletonAnimator _animator;
    private readonly GameObject _enemyGO;
    private readonly IPlayerDamageable _playerDamageable;
    private readonly NavMeshAgent _agent;
    private readonly Transform _transform;

    [Inject]
    public SkeletonStateFactory(
        ISkeletonStateMachine stateMachine,
        IPlayerDetector detector,
        EnemyStats stats,
        ISkeletonAnimator animator,
        IPlayerDamageable playerDamageable,
        NavMeshAgent agent,
        Enemy enemy)
    {
        _stateMachine = stateMachine;
        _detector = detector;
        _stats = stats;
        _animator = animator;
        _playerDamageable = playerDamageable;
        _agent = agent;
        _enemyGO = enemy.gameObject;
    }

    public IEnemyState CreateIdleState() => new SkeletonIdleState(_animator, _detector, _stateMachine, this);
    public IEnemyState CreateChaseState() => new SkeletonChaseState(_animator, _agent, _detector, _stats, _stateMachine, this) ;
    public IEnemyState CreateAttackState() => new SkeletonAttackState(_playerDamageable, _animator, _detector, _stats, _stateMachine, _agent, this);
    public IEnemyState CreateDieState() => new SkeletonDieState(_animator, _enemyGO);

    public IEnemyState CreateGetDamageState() => new SkeletonGetDamageState(_animator, _stateMachine, this);

    public IEnemyState CreateFlyState() => new SkeletonFlyState(_stateMachine, this, _transform, _animator);
}