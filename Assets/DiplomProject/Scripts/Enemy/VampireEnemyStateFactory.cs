using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

    public class VampireEnemyStateFactory : IEnemyStateFactory
{
    private readonly IEnemyAnimator _animator;
    private readonly NavMeshAgent _agent;
    private readonly IPlayerDetector _detector;
    private readonly EnemyStats _stats;
    private readonly IPlayerDamageable _playerDamageable;

    public VampireEnemyStateFactory(
        IEnemyAnimator animator,
        NavMeshAgent agent,
        IPlayerDetector detector,
        EnemyStats stats,
        IPlayerDamageable playerDamageable)
    {
        _animator = animator;
        _agent = agent;
        _detector = detector;
        _stats = stats;
        _playerDamageable = playerDamageable;
    }

    public IEnemyState CreateIdleState() => new VampireEnemyIdleState(_animator, _detector);
    public IEnemyState CreateChaseState() => new VampireEnemyChaseState(_animator, _agent, _detector, _stats);
    public IEnemyState CreateAttackState() => new VampireAttackState(_playerDamageable, _animator, _detector, _stats);
    public IEnemyState CreateDieState() => new VampireEnemyDieState(_animator);
}
