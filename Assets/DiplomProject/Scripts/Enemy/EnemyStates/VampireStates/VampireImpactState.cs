using UnityEngine;

public class VampireImpactState : IEnemyState
{
    private readonly IEnemyAnimator _animator;
    private readonly VampireEnemyStateMachine _stateMachine;
    private readonly IEnemyStateFactory _factory;

    private float _timer;


    public VampireImpactState(IEnemyAnimator animator, VampireEnemyStateMachine stateMachine, IEnemyStateFactory factory)
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
