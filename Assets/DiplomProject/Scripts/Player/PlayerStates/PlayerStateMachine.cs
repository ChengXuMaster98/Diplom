using Zenject;

public class PlayerStateMachine : ITickable
{
    private IPlayerState _currentState;

    public void SetState(IPlayerState newState)
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