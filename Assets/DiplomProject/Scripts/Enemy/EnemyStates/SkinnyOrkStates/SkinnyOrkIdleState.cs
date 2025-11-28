using UnityEngine;

public class SkinnyOrkIdleState : IEnemyState
{
    private readonly ISkinnyOrkAnimator _animator;
    private readonly ISkinnyOrkStateMachine _stateMachine;
    private readonly IPlayerDetector _detector;
    private readonly ISkinnyOrkStateFactory _stateFactory;

    private Transform _player;

    private bool _patrolStarted;

    public SkinnyOrkIdleState(ISkinnyOrkAnimator animator, IPlayerDetector detector, ISkinnyOrkStateMachine stateMachine, ISkinnyOrkStateFactory stateFactory)
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
            var chaseState = _stateFactory.CreateChaseState() as SkinnyOrkChaseState;
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
