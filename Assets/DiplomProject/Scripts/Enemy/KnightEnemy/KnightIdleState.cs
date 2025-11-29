using UnityEngine;

public class KnightIdleState : IEnemyState
{
    private readonly IKnightAnimator _animator;
    private readonly IKnightStateMachine _machine;
    private readonly IKnightStateFactory _factory;
    private readonly IPlayerDetector _detector;

    public KnightIdleState(
        IKnightAnimator animator,
        IKnightStateMachine machine,
        IKnightStateFactory factory,
        IPlayerDetector detector)
    {
        _animator = animator;
        _machine = machine;
        _factory = factory;
        _detector = detector;
    }

    public void Enter()
    {
        _animator.PlayIdle();
    }

    public void Tick()
    {
        if (_detector.Player != null)
        {
            // если игрок далеко Ч подходим, если близко Ч кружим
            float dist = Vector3.Distance(
                _animator.Transform.position,
                _detector.Player.position);

            if (dist > 3f)
                _machine.SetState(_factory.CreateChaseState());   // подход
            else
                _machine.SetState(_factory.CreateCircleState());  // дуэльное кружение
        }
    }

    public void Exit() { }
}