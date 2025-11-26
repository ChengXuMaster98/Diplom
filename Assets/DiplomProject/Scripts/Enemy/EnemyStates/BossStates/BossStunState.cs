using UnityEngine;
using UnityEngine.AI;

public class BossStunState : IEnemyState
{
    private readonly IBossAnimator _animator;
    private readonly IBossStateMachine _stateMachine;
    private readonly NavMeshAgent _agent;

    private readonly IEnemyState _resumeState;
    private readonly float _duration;

    private float _timer;

    public BossStunState(
        IBossAnimator animator,
        IBossStateMachine machine,
        NavMeshAgent agent,
        IEnemyState resumeState,
        float duration)
    {
        _animator = animator;
        _stateMachine = machine;
        _agent = agent;
        _resumeState = resumeState;
        _duration = duration;
    }

    public void Enter()
    {
        Debug.Log($"[STUN] Enter({_duration})");
        _timer = 0;

        if (_agent != null)
            _agent.isStopped = true;

        _animator.PlayStun();
    }

    public void Tick()
    {

        _timer += Time.deltaTime;
        if (_timer < _duration) return;

        if (_agent != null)
            _agent.isStopped = false;

        _stateMachine.RevertToPreviousState(); // возврат куда должен
    }

    public void Exit()
    {

    }
}