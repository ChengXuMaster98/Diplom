using UnityEngine;
using Zenject;

public class VampireEnemyStateMachine : IEnemyStateMachine
{
    public IEnemyState CurrentState { get; private set; }
    private IEnemyState _dieState;
    private IEnemyState _previousState;

    private bool _locked = false;


    public void Initialize(IEnemyState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }



    public void SetState(IEnemyState newState)
    {
        if (_locked)
            return;

            CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Tick()
    {
        CurrentState?.Tick();
    }

    public void SetToDieState()
    {
        CurrentState?.Exit();
        CurrentState = _dieState;
        CurrentState.Enter();
    }

    public void RevertToPreviousState()
    {
        if (_previousState == null)
            return;

        var target = _previousState;
        _previousState = CurrentState;
        CurrentState?.Exit();
        CurrentState = target;
        CurrentState.Enter();
    }
}