using UnityEngine;

    public class SkeletonIdleState : IEnemyState
{
    private readonly ISkeletonAnimator _animator;
    private readonly ISkeletonStateMachine _stateMachine;
    private readonly IPlayerDetector _detector;
    private readonly ISkeletonStateFactory _stateFactory;

    private Transform _player;

    private bool _patrolStarted;

    public SkeletonIdleState(ISkeletonAnimator animator, IPlayerDetector detector, ISkeletonStateMachine stateMachine, ISkeletonStateFactory stateFactory)
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
            var chaseState = _stateFactory.CreateChaseState() as SkeletonChaseState;
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
