using UnityEngine;

public class KnightGetDamageState : IEnemyState
{
    private readonly IKnightAnimator _animator;
    private readonly IKnightStateMachine _machine;
    private readonly IKnightStateFactory _factory;
    private float _timer;

    public KnightGetDamageState(IKnightAnimator animator, IKnightStateMachine machine, IKnightStateFactory factory)
    {
        _animator = animator;
        _machine = machine;
        _factory = factory;
    }

    public void Enter()
    {
        _timer = 0.4f;
        _animator.PlayImpact();
    }

    public void Tick()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _machine.SetState(_factory.CreateCircleState());
        }
    }

    public void Exit() { }
}