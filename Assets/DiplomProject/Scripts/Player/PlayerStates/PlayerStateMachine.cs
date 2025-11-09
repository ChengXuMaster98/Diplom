using System.Diagnostics;
using Zenject;

public class PlayerStateMachine : ITickable
{
    private IPlayerState _currentState;
    private IPlayerState _previousState;

    public IPlayerState CurrentState => _currentState;

    public void SetState(IPlayerState newState)
    {
        if (_currentState != null && !_currentState.CanExit())
        {
            //UnityEngine.Debug.Log($"[PlayerStateMachine] Не могу выйти из состояния: {_currentState.GetType().Name} — CanExit() == false");
            return;
        }

        if (_currentState == newState)
        {
            return;
        }
        //UnityEngine.Debug.Log($"[PlayerStateMachine] Попытка установить то же самое состояние: {newState.GetType().Name}");

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