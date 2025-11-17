using UnityEngine;

public class SkeletonStateMachine : ISkeletonStateMachine
{
    public IEnemyState CurrentState { get; private set; }
    private IEnemyState _dieState;

    public void Initialize(IEnemyState startState)
    {
        CurrentState = startState;
        CurrentState.Enter();
    }


    public void SetState(IEnemyState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }

    public void Tick()
    {
        CurrentState?.Tick();
    }

    public void SetToDieState()
    {
        CurrentState?.Tick();
    }

}