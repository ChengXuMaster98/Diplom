using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyAI : MonoBehaviour, IInitializable, ITickable
{
    public VampireEnemyStateMachine _stateMachine;
    public IEnemyStateFactory _stateFactory;
    public IPlayerDetector _playerDetector;

    private IEnemyState _idleState;
    private IEnemyState _chaseState;
    private IEnemyState _attackState;
    private IEnemyState _dieState;
    
    
    private EnemyStats _enemyStats;

    private Transform _targetPlayer;
    private Enemy _enemy;

    [Inject]
    public void Construct(VampireEnemyStateMachine stateMachine, IEnemyStateFactory stateFactory, IPlayerDetector playerDetector, EnemyStats enemyStats, Enemy enemy)
    {
        _stateMachine = stateMachine;
        _stateFactory = stateFactory;
        _playerDetector = playerDetector;
        _enemyStats = enemyStats;
        _enemy = enemy;

        _playerDetector.PlayerDetected += OnPlayerDetected;
        _playerDetector.PlayerLost += OnPlayerLost;
    }

    public void Initialize()
    {
        Debug.Log($"EnemyAI Initialize: StateMachine null? {_stateMachine == null}, Factory null? {_stateFactory == null}, Detector null? {_playerDetector == null}");
        _idleState = _stateFactory.CreateIdleState();
        _chaseState = _stateFactory.CreateChaseState();
        _attackState = _stateFactory.CreateAttackState();
        _dieState = _stateFactory.CreateDieState();

        _stateMachine.Initialize(_stateFactory.CreateIdleState());
    }

    private void OnPlayerDetected(Transform player)
    {
        Debug.Log(">> OnPlayerDetected called with: " + player.name);
        _targetPlayer = player;
        _stateMachine.SetState(_chaseState);
    }

    private void OnPlayerLost()
    {
        Debug.Log("Player lost!");
        _targetPlayer = null;
        _stateMachine.SetState(_idleState);
    }


    public void Tick()
    {

        if (_enemy.IsDead)
        {
            _stateMachine.SetState(_dieState);
            return;
        }

        if (_targetPlayer != null)
        {
            
            float distance = Vector3.Distance(transform.position, _targetPlayer.position);

            if (distance <= _enemyStats.AttackRange)
            {
                _stateMachine.SetState(_attackState);
            }
            else if (_stateMachine.CurrentState == _attackState && distance > _enemyStats.AttackRange)
            {
                _stateMachine.SetState(_chaseState);
            }
        }

        _stateMachine.Tick();
    }
}