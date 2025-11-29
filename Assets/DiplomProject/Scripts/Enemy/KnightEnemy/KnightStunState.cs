using UnityEngine;
using UnityEngine.AI;

public class KnightStunState : IEnemyState
{
    private readonly IKnightAnimator _animator;
    private readonly IKnightStateMachine _machine;
    private readonly NavMeshAgent _agent;
    private readonly IEnemyState _resumeState;
    private readonly float _duration;
    private float _timer;

    public KnightStunState(
        IKnightAnimator animator,
        IKnightStateMachine machine,
        NavMeshAgent agent,
        IEnemyState resumeState,
        float duration)
    {
        _animator = animator;
        _machine = machine;
        _agent = agent;
        _resumeState = resumeState;
        _duration = duration;
    }

    public void Enter()
    {
        _timer = 0f;
        if (_agent != null) _agent.isStopped = true;
        _animator.PlayStun();
    }

    public void Tick()
    {
        _timer += Time.deltaTime;
        if (_timer >= _duration)
        {
            if (_agent != null) _agent.isStopped = false;
            _machine.SetState(_resumeState);
        }
    }

    public void Exit() { }
}
