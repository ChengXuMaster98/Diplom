using UnityEngine;

public class PlayerFallState : IPlayerState
{
    private readonly Animator _animator;
    private readonly CharacterMovementController _movement;

    public PlayerFallState(Animator animator, CharacterMovementController movement)
    {
        _animator = animator;
        _movement = movement;
    }

    public void Enter()
    {
        _animator.SetBool("IsFalling", true);
    }

    public void Tick() { }

    public void Exit()
    {
        //_animator.SetBool("IsFalling", false);
    }

    public bool CanExit()
    {
        return _movement.IsGrounded && _movement.VerticalVelocity <= 0; // приземлились 
    }
}