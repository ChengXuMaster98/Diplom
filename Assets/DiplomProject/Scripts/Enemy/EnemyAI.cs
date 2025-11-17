using TMPro;
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
    private IEnemyState _getImpactState;

    private bool _isDead = false;
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

        _enemy.OnDamaged += OnDamaged;
        _enemy.OnDeath += HandleDeath;
    }

    public void Initialize()
    {
        Debug.Log($"EnemyAI Initialize: StateMachine null? {_stateMachine == null}, Factory null? {_stateFactory == null}, Detector null? {_playerDetector == null}");
        _idleState = _stateFactory.CreateIdleState();
        _chaseState = _stateFactory.CreateChaseState();
        _attackState = _stateFactory.CreateAttackState();
        _dieState = _stateFactory.CreateDieState();
        _getImpactState = _stateFactory.CreateGetDamageState();


        var idleState = _stateFactory.CreateIdleState();
        _stateMachine.Initialize(idleState);
    }

    private void HandleDeath()
    {
        if (_isDead)
            return;

        _isDead = true;

        // создаём dieState один раз
        _dieState = _stateFactory.CreateDieState();

        // Переходим в состояние смерти
        _stateMachine.SetState(_dieState);

        // Отписываемся, чтобы не было утечек
        _playerDetector.PlayerDetected -= OnPlayerDetected;
        _playerDetector.PlayerLost -= OnPlayerLost;
    }

    private void OnDamaged()
    {
        if (!_enemy.IsDead)
            _stateMachine.SetState(_stateFactory.CreateGetDamageState());
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

        if (_isDead)
        {
            // Мёртвый враг не тикает
            return;
        }

        _stateMachine.Tick();
    }
}