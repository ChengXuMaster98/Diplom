

public class SkeletonStateMachine : ISkeletonStateMachine
{
    public IEnemyState CurrentState { get; private set; }
    private IEnemyState _dieState;
    private IEnemyState _previousState;

    public void Initialize(IEnemyState startState)
    {
        CurrentState = startState;
        CurrentState.Enter();
    }


    public void SetState(IEnemyState newState)
    {

        if (newState == null)
            return;


        if (CurrentState == newState)
            return;

        _previousState = CurrentState;
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