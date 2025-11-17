using UnityEngine;
using Zenject;

public class VampireEnemyStateMachine : IEnemyStateMachine
{
    public IEnemyState CurrentState { get; private set; }
    private IEnemyState _dieState;


    public void Initialize(IEnemyState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    //public void Initialize(IEnemyState idle, IEnemyState chase, IEnemyState attack, IEnemyState die)
    //{
    //    CurrentState = idle;
    //    CurrentState.Enter();
    //}


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

    public void SetToDieState()
    {
        CurrentState?.Exit();
        CurrentState = _dieState;
        CurrentState.Enter();
    }
}