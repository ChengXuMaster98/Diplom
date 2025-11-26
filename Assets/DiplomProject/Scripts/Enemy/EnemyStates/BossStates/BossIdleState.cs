using UnityEngine;

public class BossIdleState : IEnemyState
{
    private readonly IBossAnimator _animator;
    private readonly IBossStateMachine _stateMachine;
    private readonly IPlayerDetector _detector;
    private readonly IBossStateFactory _stateFactory;

    private Transform _player;

    private bool _patrolStarted;

    public BossIdleState(IBossAnimator animator, IPlayerDetector detector, IBossStateMachine stateMachine, IBossStateFactory stateFactory)
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
        if (!_patrolStarted)
        {
            _patrolStarted = true;
            _stateMachine.SetState(_stateFactory.CreatePatrolState());
            return;
        }


        if (_player != null)
        {
            var chaseState = _stateFactory.CreateChaseState() as BossChaseState;
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
