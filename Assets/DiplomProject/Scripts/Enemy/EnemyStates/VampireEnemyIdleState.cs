using UnityEngine;
using Zenject;

public class VampireEnemyIdleState : IEnemyState
{
    private readonly IEnemyAnimator _animator;
    private readonly VampireEnemyStateMachine _stateMachine;
    private readonly IPlayerDetector _detector;
    private readonly IEnemyStateFactory _stateFactory;

    private Transform _player;

    public VampireEnemyIdleState(IEnemyAnimator animator, IPlayerDetector detector, VampireEnemyStateMachine stateMachine, IEnemyStateFactory stateFactory)
    {
        _animator = animator;
        _detector = detector;
        _stateMachine = stateMachine;
        _stateFactory = stateFactory;

        _detector.PlayerDetected += OnPlayerDetected;
    }

    public void Enter()
    {
        _animator.PlayIdle();
    }
    public void Tick()
    {
        if (_player != null)
        {
            var chaseState = _stateFactory.CreateChaseState() as VampireEnemyChaseState;
            _stateMachine.SetState(chaseState);
        }
    }
    public void Exit()
    {
        _detector.PlayerDetected -= OnPlayerDetected;
    }

    private void OnPlayerDetected(Transform player)
    {
        _player = player;
    }
}