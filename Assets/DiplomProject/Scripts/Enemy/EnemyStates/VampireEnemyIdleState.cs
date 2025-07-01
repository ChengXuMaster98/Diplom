using UnityEngine;
using Zenject;

public class VampireEnemyIdleState : IEnemyState
{
    private readonly IEnemyAnimator _animator;
    private readonly VampireEnemyStateMachine _stateMachine;


    public VampireEnemyIdleState(IEnemyAnimator animator, IPlayerDetector detector)
    {
        _animator = animator;
    }

    public void Enter()
    {
        _animator.PlayIdle();
    }
    public void Tick()
    {

    }
    public void Exit()
    {
    }
}
