using UnityEngine;
using Zenject;

public class VampireEnemyIdleState : IEnemyState
{
    private readonly IEnemyAnimator _animator;
    private readonly VampireEnemyStateMachine _stateMachine;
    private readonly IPlayerDetector _detector;


    public VampireEnemyIdleState(IEnemyAnimator animator, IPlayerDetector detector)
    {
        _animator = animator;
        _detector = detector;
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
