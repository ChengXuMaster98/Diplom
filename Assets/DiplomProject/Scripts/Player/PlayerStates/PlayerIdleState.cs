using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    private readonly Animator _animator;

    public PlayerIdleState(Animator animator)
    {
        _animator = animator;
    }

    public void Enter()
    {

    }

    public void Tick() { }

    public void Exit() { }
}
