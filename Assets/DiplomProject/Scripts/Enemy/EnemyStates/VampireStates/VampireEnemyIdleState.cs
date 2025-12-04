using UnityEngine;

public class VampireEnemyIdleState : IEnemyState
{
    private readonly IEnemyAnimator _animator;
    private readonly VampireEnemyStateMachine _stateMachine;
    private readonly IPlayerDetector _detector;
    private readonly IEnemyStateFactory _stateFactory;

    private Transform _player;

    EnemySoundController _sound;
    private float _vocalTimer;

    public VampireEnemyIdleState(IEnemyAnimator animator, IPlayerDetector detector, VampireEnemyStateMachine stateMachine, IEnemyStateFactory stateFactory, EnemySoundController sound)
    {
        _animator = animator;
        _detector = detector;
        _stateMachine = stateMachine;
        _stateFactory = stateFactory;

        _detector.PlayerDetected += OnPlayerDetected;
        _sound = sound;
    }

    public void Enter()
    {
        _animator.PlayIdle();
        _vocalTimer = _sound.GetRandomIdleInterval();

    }
    public void Tick()
    {
        if (_player != null)
        {
            var chaseState = _stateFactory.CreateChaseState() as VampireEnemyChaseState;
            _stateMachine.SetState(chaseState);
        }

        _vocalTimer -= Time.deltaTime;
        if (_vocalTimer <= 0f)
        {
            _sound.PlayIdle();
            _vocalTimer = _sound.GetRandomIdleInterval();
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