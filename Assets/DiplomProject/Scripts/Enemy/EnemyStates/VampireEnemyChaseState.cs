using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class VampireEnemyChaseState : IEnemyState
{
    private readonly NavMeshAgent _agent;
    private readonly IEnemyAnimator _animator;
    private Transform _targetPlayer;
    private readonly IPlayerDetector _detector;
    private EnemyStats _enemyStats;

    public VampireEnemyChaseState(IEnemyAnimator animator, NavMeshAgent agent, IPlayerDetector detector, EnemyStats enemyStats)
    {
        _agent = agent;
        _animator = animator;
        _detector = detector;
        _enemyStats = enemyStats;
    }

    public void SetTarget(Transform player)
    {
        _targetPlayer = player;
    }

    public void Enter()
    {
        _animator.PlayChase();
        _agent.isStopped = false;

    }

    public void Tick()
    {
        if (_targetPlayer != null)
            _agent.SetDestination(_targetPlayer.position);
    }

    public void Exit()
    {
        //_agent.ResetPath();
    }
}