using System.Diagnostics;
using Zenject;

public class PlayerStateMachine : ITickable
{
    private IPlayerState _currentState;
    private IPlayerState _previousState;

    public IPlayerState CurrentState => _currentState;

    public void SetState(IPlayerState newState)
    {
        _previousState = _currentState;
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void RevertToPreviousState()
    {
        if (_previousState != null)
        {
            SetState(_previousState);
        }
    }
    public void Tick()
    {
        _currentState?.Tick();
    }
}