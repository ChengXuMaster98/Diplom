using UnityEngine;
using Zenject;

public class SkeletonStateFactory : ISkeletonStateFactory
{
    private readonly Enemy _enemy;
    private readonly ISkeletonStateMachine _stateMachine;
    private readonly IPlayerDetector _detector;
    private readonly EnemyStats _stats;
    private readonly Animator _animator;
    private readonly Transform _transform;

    [Inject]
    public SkeletonStateFactory(
        Enemy enemy,
        ISkeletonStateMachine stateMachine,
        IPlayerDetector detector,
        EnemyStats stats,
        Animator animator)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
        _detector = detector;
        _stats = stats;
        _animator = animator;
        _transform = enemy.transform;
    }

    public IEnemyState CreateIdleState() => new SkeletonIdleState(_stateMachine, _detector, this, _transform, _animator);
    public IEnemyState CreateChaseState() => new SkeletonChaseState(_stateMachine, this, _transform, _detector, _animator, _stats);
    public IEnemyState CreateAttackState() => new SkeletonAttackState(_stateMachine, this, _transform, _animator, _stats);
    //public IEnemyState CreateFloatState() => new SkeletonFlyState(_stateMachine, this, _transform, _animator);
    public IEnemyState CreateDieState() => new SkeletonDieState(_stateMachine, this, _transform, _animator);

    public IEnemyState CreateGetDamageState() => new SkeletonGetDamageState(_animator, _stateMachine, this);

    public IEnemyState CreateFlyState() => new SkeletonFlyState(_stateMachine, this, _transform, _animator);
}