using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class SkinnyOrkStateFactory : ISkinnyOrkStateFactory
{

    private readonly ISkinnyOrkStateMachine _stateMachine;
    private readonly IPlayerDetector _detector;
    private readonly EnemyStats _stats;
    private readonly ISkinnyOrkAnimator _animator;
    private readonly GameObject _enemyGO;
    private readonly IPlayerDamageable _playerDamageable;
    private readonly NavMeshAgent _agent;
    private readonly Transform _transform;
    private readonly EnemySoundController _sound;

    [Inject]
    public SkinnyOrkStateFactory(
        ISkinnyOrkStateMachine stateMachine,
        IPlayerDetector detector,
        EnemyStats stats,
        ISkinnyOrkAnimator animator,
        IPlayerDamageable playerDamageable,
        NavMeshAgent agent,
        Enemy enemy,
        EnemySoundController sound)
    {
        _stateMachine = stateMachine;
        _detector = detector;
        _stats = stats;
        _animator = animator;
        _playerDamageable = playerDamageable;
        _agent = agent;
        _enemyGO = enemy.gameObject;
        _sound = sound;
    }

    public IEnemyState CreateIdleState() => new SkinnyOrkIdleState(_animator, _detector, _stateMachine, this);
    public IEnemyState CreateChaseState() => new SkinnyOrkChaseState(_animator, _agent, _detector, _stats, _stateMachine, this, _sound);
    public IEnemyState CreateAttackState() => new SkinnyOrkAttackState(_playerDamageable, _animator, _detector, _stats, _stateMachine, _agent, this, _sound);
    public IEnemyState CreateDieState() => new SkinnyOrkDieState(_animator, _enemyGO);

    public IEnemyState CreateGetDamageState() => new SkinnyOrkGetDamageState(_animator, _stateMachine, this);


    public IEnemyState CreateStunState(float duration)
    {
        var resume = _stateMachine.CurrentState;
        return new SkinnyOrkStunState(_animator, _stateMachine, _agent, resume, duration);
    }

    public IEnemyState CreatePatrolState()
        => new SkinnyOrkPatrolState(_animator, _stateMachine, this, _detector, _agent, _sound);
}