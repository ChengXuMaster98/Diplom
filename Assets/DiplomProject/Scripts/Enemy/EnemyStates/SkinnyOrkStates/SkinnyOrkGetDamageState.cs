using UnityEngine;

public class SkinnyOrkGetDamageState : IEnemyState
{
    private readonly ISkinnyOrkAnimator _animator;
    private readonly ISkinnyOrkStateMachine _stateMachine;
    private readonly ISkinnyOrkStateFactory _factory;

    private float _timer;


    public SkinnyOrkGetDamageState(ISkinnyOrkAnimator animator, ISkinnyOrkStateMachine stateMachine, ISkinnyOrkStateFactory factory)
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