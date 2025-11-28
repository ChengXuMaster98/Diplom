using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class BossStateFactory : IBossStateFactory
{

    private readonly IBossStateMachine _stateMachine;
    private readonly IPlayerDetector _detector;
    private readonly EnemyStats _stats;
    private readonly IBossAnimator _animator;
    private readonly GameObject _enemyGO;
    private readonly IPlayerDamageable _playerDamageable;
    private readonly NavMeshAgent _agent;
    private readonly Transform _transform;
    private readonly GameWonUI _gameWonUI;

    [Inject]
    public BossStateFactory(
        IBossStateMachine stateMachine,
        IPlayerDetector detector,
        EnemyStats stats,
        IBossAnimator animator,
        IPlayerDamageable playerDamageable,
        NavMeshAgent agent,
        Enemy enemy,
        GameWonUI gameWonUI)
    {
        _stateMachine = stateMachine;
        _detector = detector;
        _stats = stats;
        _animator = animator;
        _playerDamageable = playerDamageable;
        _agent = agent;
        _enemyGO = enemy.gameObject;
        _gameWonUI = gameWonUI;
    }

    public IEnemyState CreateIdleState() => new BossIdleState(_animator, _detector, _stateMachine, this);
    public IEnemyState CreateChaseState() => new BossChaseState(_animator, _agent, _detector, _stats, _stateMachine, this);
    public IEnemyState CreateAttackState() => new BossAttackState(_playerDamageable, _animator, _detector, _stats, _stateMachine, _agent, this);
    public IEnemyState CreateDieState() => new BossDieState(_animator, _enemyGO, _gameWonUI);

    public IEnemyState CreateGetDamageState() => new BossGetDamageState(_animator, _stateMachine, this);

    public IEnemyState CreateStunState(float duration)
    {
        var resume = _stateMachine.CurrentState;
        return new BossStunState(_animator, _stateMachine, _agent, resume, duration);
    }

    public IEnemyState CreatePatrolState()
        => new BossPatrolState(_animator, _stateMachine, this, _detector, _agent);
}