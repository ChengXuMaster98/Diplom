using System.Collections;
using System.Collections.Generic;
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

    [Inject]
    public virtual void Construct(
        TStateMachine stateMachine,
        TStateFactory stateFactory,
        IPlayerDetector playerDetector,
        Enemy enemy)
    {
        _stateMachine = stateMachine;
        _stateFactory = stateFactory;
        _playerDetector = playerDetector;
        _enemy = enemy;

        _playerDetector.PlayerDetected += OnPlayerDetected;
        _playerDetector.PlayerLost += OnPlayerLost;
    }


    protected abstract void OnPlayerDetected(Transform player);
    protected abstract void OnPlayerLost();

    public virtual void Initialize() { }

    public virtual void Tick()
    {
        if (_enemy.IsDead)
        {
            _stateMachine.SetToDieState();
            return;
        }

        _stateMachine.Tick();
    }
}