using UnityEngine;

public class BossGetDamageState : IEnemyState
{
    private readonly IBossAnimator _animator;
    private readonly IBossStateMachine _stateMachine;
    private readonly IBossStateFactory _factory;

    private float _timer;


    public BossGetDamageState(IBossAnimator animator, IBossStateMachine stateMachine, IBossStateFactory factory)
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