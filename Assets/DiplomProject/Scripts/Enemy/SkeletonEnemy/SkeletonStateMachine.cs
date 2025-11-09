using UnityEngine;

public class SkeletonStateMachine : ISkeletonStateMachine
{
    private IEnemyState _currentState;
    private IEnemyState _dieState;

    public void Initialize(IEnemyState startState)
    {
        _currentState = startState;
        _currentState.Enter();
    }


    public void SetState(IEnemyState newState)
    {
        if (_currentState == newState)
            return;

        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    public void Tick()
    {
        _currentState?.Tick();
    }

    public void SetToDieState()
    {
        _currentState?.Exit();
        _currentState = _dieState;
        _currentState.Enter();
    }

}