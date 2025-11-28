using UnityEngine;
using Zenject;

    public abstract class EnemyAIBase<TStateMachine, TStateFactory> : MonoBehaviour, IInitializable, ITickable
    where TStateFactory : class, IStateFactory
    where TStateMachine : class, IStateMachine
{
    protected TStateMachine _stateMachine;
    protected TStateFactory _stateFactory;
    protected IPlayerDetector _playerDetector;
    protected Enemy _enemy;
    
    private bool _isDead = false;
    private IEnemyState _dieState;

    protected EnemyAggroTracker _aggroTracker;
    protected bool _isAggro;

    [Inject]
    public virtual void Construct(
        TStateMachine stateMachine,
        TStateFactory stateFactory,
        IPlayerDetector playerDetector,
        Enemy enemy,
        EnemyAggroTracker aggroTracker)
    {
        _stateMachine = stateMachine;
        _stateFactory = stateFactory;
        _playerDetector = playerDetector;
        _enemy = enemy;
        _aggroTracker = aggroTracker;

        _playerDetector.PlayerDetected += OnPlayerDetected;
        _playerDetector.PlayerLost += OnPlayerLost;

        _enemy.OnDamaged += OnDamaged;

        _enemy.OnDeath += HandleDeath;
    }

    private void HandleDeath()
    {
        if (_isDead)
            return;

        _isDead = true;


        //если враг умер во время агро — безопасно декрементим
        if (_isAggro)
        {
            _isAggro = false;
            _aggroTracker?.Decrement();
        }



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
        if (_enemy.IsStunned)
            return;

        if (!_enemy.IsDead)
            _stateMachine.SetState(_stateFactory.CreateGetDamageState());
    }


    protected abstract void OnPlayerDetected(Transform player);
    protected abstract void OnPlayerLost();

    public virtual void Initialize() { }

    public virtual void Tick()
    {
        if (_isDead)
        {
            // Мёртвый враг не тикает
            return;
        }

        _stateMachine.Tick();
    }
}