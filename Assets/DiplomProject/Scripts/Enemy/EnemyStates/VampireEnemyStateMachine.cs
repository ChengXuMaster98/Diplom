using UnityEngine;
using Zenject;

public class VampireEnemyStateMachine : StateMachineBehaviour, ITickable
{
    public IEnemyState CurrentState { get; private set; }

    public void Initialize(IEnemyState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void SetState(IEnemyState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Tick()
    {
        CurrentState?.Tick();
    }
}