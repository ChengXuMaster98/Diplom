using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : IEnemyState
{
    private readonly NavMeshAgent _agent;
    private readonly IEnemyAnimator _animator;
    private Transform _target;

    public EnemyChaseState(NavMeshAgent agent, IEnemyAnimator animator)
    {
        _agent = agent;
        _animator = animator;
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