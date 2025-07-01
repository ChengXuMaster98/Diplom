using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public class VampireEnemyChaseState : IEnemyState
{
    private readonly NavMeshAgent _agent;
    private readonly IEnemyAnimator _animator;
    private Transform _target;
    private readonly IPlayerDetector _detector;
    private EnemyStats _enemyStats;

    public VampireEnemyChaseState(IEnemyAnimator animator, NavMeshAgent agent, IPlayerDetector detector, EnemyStats enemyStats)
    {
        _agent = agent;
        _animator = animator;
        _detector = detector;
        _enemyStats = enemyStats;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void Enter()
    {
        _animator.PlayChase();
        _agent.isStopped = false;
    }

    public void Tick()
    {
        if (_target != null)
            _agent.SetDestination(_target.position);
    }

    public void Exit()
    {
        _agent.ResetPath();
    }
}