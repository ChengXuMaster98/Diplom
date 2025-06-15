using UnityEngine;
using Zenject;

public class EnemyStateMachine : StateMachineBehaviour, ITickable
{
    private IEnemyState _currentState;

    public void Initialize(IEnemyState startingState)
    {
        _currentState = startingState;
        _currentState.Enter();
    }

    public void SetState(IEnemyState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void Tick()
    {
        _currentState?.Tick();
    }
}