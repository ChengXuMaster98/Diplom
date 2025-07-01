using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyAI : MonoBehaviour, IInitializable, ITickable
{
    private readonly VampireEnemyStateMachine _stateMachine;
    private readonly IEnemyStateFactory _stateFactory;
    private readonly IPlayerDetector _playerDetector;

    private IEnemyState _idleState;
    private IEnemyState _chaseState;
    private IEnemyState _attackState;
    private IEnemyState _dieState;

    private Transform _targetPlayer;
    private Enemy _enemy;

    [Inject]
    public EnemyAI(VampireEnemyStateMachine stateMachine, IEnemyStateFactory stateFactory, IPlayerDetector playerDetector)
    {
        _stateMachine = stateMachine;
        _stateFactory = stateFactory;
        _playerDetector = playerDetector;

    }

    public void Initialize()
    {
        _idleState = _stateFactory.CreateIdleState();
        _chaseState = _stateFactory.CreateChaseState();
        _attackState = _stateFactory.CreateAttackState();
        _dieState = _stateFactory.CreateDieState();

        _stateMachine.Initialize(_idleState);

        _playerDetector.PlayerDetected += OnPlayerDetected;
        _playerDetector.PlayerLost += OnPlayerLost;
    }

    private void OnPlayerDetected(Transform player)
    {
        _targetPlayer = player;
        _stateMachine.SetState(_chaseState);
    }

    private void OnPlayerLost()
    {
        _targetPlayer = null;
        _stateMachine.SetState(_idleState);
    }


    public void Tick()
    {
        if (_enemy == null)
            _enemy = GetComponent<Enemy>();


        if (_enemy.IsDead)
        {
            _stateMachine.SetState(_dieState);
            return;
        }

        _stateMachine.Tick();

        if (_targetPlayer != null)
        {
            float distance = Vector3.Distance(transform.position, _targetPlayer.position);

            if (distance < 1.5f)
            {
                _stateMachine.SetState(_attackState);
            }
            else if (_stateMachine.CurrentState == _attackState && distance > 1.5f)
            {
                _stateMachine.SetState(_chaseState);
            }              
        }
    }
}