using UnityEngine;
using UnityEngine.AI;

public class VampireEnemyStateFactory : IEnemyStateFactory
{
    private readonly IEnemyAnimator _animator;
    private readonly NavMeshAgent _agent;
    private readonly IPlayerDetector _detector;
    private readonly EnemyStats _stats;
    private readonly IPlayerDamageable _playerDamageable;
    private readonly VampireEnemyStateMachine _stateMachine;
    private readonly GameObject _enemyGO;
    private readonly EnemySoundController _sound;

    public VampireEnemyStateFactory(
        IEnemyAnimator animator,
        NavMeshAgent agent,
        IPlayerDetector detector,
        EnemyStats stats,
        IPlayerDamageable playerDamageable,
        VampireEnemyStateMachine stateMachine,
        Enemy enemy,
        EnemySoundController sound)
    {
        _animator = animator;
        _agent = agent;
        _detector = detector;
        _stats = stats;
        _playerDamageable = playerDamageable;
        _stateMachine = stateMachine;
        _enemyGO = enemy.gameObject;
        _sound = sound;
    }

    public IEnemyState CreateIdleState() => new VampireEnemyIdleState(_animator, _detector, _stateMachine, this, _sound);
    public IEnemyState CreateChaseState() => new VampireEnemyChaseState(_animator, _agent, _detector, _stats, _stateMachine, this, _sound);
    public IEnemyState CreateAttackState() => new VampireEnemyAttackState(_playerDamageable, _animator, _detector, _stats, _stateMachine, _agent, this, _sound);
    public IEnemyState CreateDieState() => new VampireEnemyDieState(_animator, _enemyGO);
    public IEnemyState CreateGetDamageState() => new VampireImpactState(_animator, _stateMachine, this);

    public IEnemyState CreateStunState(float duration)
    {
        var resume = _stateMachine.CurrentState;
        return new VampireEnemyStunState(_animator, _stateMachine, _agent, resume, duration);
    }
}