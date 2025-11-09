using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerJumpState : IPlayerState
{
    private static readonly int Jump = Animator.StringToHash("Jump");
    private readonly Animator _animator;
    private readonly CharacterMovementController _movement;

    public PlayerJumpState(Animator animator, CharacterMovementController movement)
    {
        _animator = animator;
        _movement = movement;
    }

    public void Enter()
    {
        _animator.SetTrigger("IsJumping");
        _movement.Jump();
    }

    public void Tick()
    {
        _animator.SetBool("IsGrounded", _movement.IsGrounded);
        _animator.SetBool("IsFalling", _movement.VerticalVelocity <= 0 && !_movement.IsGrounded);
    }

    public void Exit()
    {
        //_animator.SetTrigger("IsJumping");
    }

    public bool CanExit()
    {
        // Can only exit when landed
        return _movement.VerticalVelocity <= 0 && !_movement.IsGrounded;
    }
}