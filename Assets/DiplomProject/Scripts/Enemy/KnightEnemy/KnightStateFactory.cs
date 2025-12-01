using UnityEngine.AI;
using Zenject;

public class KnightStateFactory : IKnightStateFactory
{
    private readonly IKnightStateMachine _machine;
    private readonly IPlayerDetector _detector;
    private readonly EnemyStats _stats;
    private readonly IKnightAnimator _animator;
    private readonly IPlayerDamageable _playerDamageable;
    private readonly NavMeshAgent _agent;
    private readonly Enemy _enemy;

    [Inject]
    public KnightStateFactory(
        IKnightStateMachine machine,
        IPlayerDetector detector,
        EnemyStats stats,
        IKnightAnimator animator,
        IPlayerDamageable playerDamageable,
        NavMeshAgent agent,
        Enemy enemy)
    {
        _machine = machine;
        _detector = detector;
        _stats = stats;
        _animator = animator;
        _playerDamageable = playerDamageable;
        _agent = agent;
        _enemy = enemy;
    }

    public IEnemyState CreateIdleState()
        => new KnightIdleState(_animator, _machine, this, _detector);

    public IEnemyState CreateChaseState()
        => new KnightApproachState(_animator, _machine, this, _detector, _agent, _stats);

    public IEnemyState CreateAttackState()
        => new KnightAttackState(_animator, _machine, this, _detector, _playerDamageable, _stats, _agent);

    public IEnemyState CreateDieState()
        => new KnightDieState(_animator, _enemy.gameObject);

    public IEnemyState CreateGetDamageState()
        => new KnightGetDamageState(_animator, _machine, this);

    public IEnemyState CreateStunState(float duration)
    {
        var resume = _machine.CurrentState;
        return new KnightStunState(_animator, _machine, _agent, resume, duration);
    }

    public IEnemyState CreatePatrolState()
    {
        
        return CreateIdleState();
    }

    public IEnemyState CreateCircleState()
        => new KnightCircleState(_animator, _machine, this, _detector, _agent, _stats);

    public IEnemyState CreateRetreatState()
        => new KnightRetreatState(_animator, _machine, this, _detector, _agent);
}