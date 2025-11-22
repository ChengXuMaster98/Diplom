using UnityEngine;

public class SkeletonGetDamageState : IEnemyState
{
    private readonly ISkeletonAnimator _animator;
    private readonly ISkeletonStateMachine _stateMachine;
    private readonly ISkeletonStateFactory _factory;

    private float _timer;


    public SkeletonGetDamageState(ISkeletonAnimator animator, ISkeletonStateMachine stateMachine, ISkeletonStateFactory factory)
    {
        _animator = animator;
        _stateMachine = stateMachine;
        _factory = factory;

    }

    public void Enter()
    {
        _timer = 0.5f;   // длительность анимации получения урона
        
        
        _animator.PlayImpact();
    }

    public void Exit()
    {

    }

    public void Tick()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            // Возврат в Idle или Chase
            _stateMachine.SetState(_factory.CreateChaseState());
        }
    }
}