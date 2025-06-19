using UnityEngine;

public class PlayerJumpState : IPlayerState
{
    private readonly Animator _animator;
    private readonly CharacterMovementController _movement;
    private float _jumpForce = 5f;
    private float _gravity = -9.8f;
    private float _verticalVelocity;
    private bool _isGrounded;

    public PlayerJumpState(Animator animator, CharacterMovementController movement)
    {
        _animator = animator;
        _movement = movement;
    }

    public bool CanExit()
    {
        // Can only exit when landed
        return _isGrounded;
    }

    public void Enter()
    {
        _animator.SetTrigger("Jump");
        _verticalVelocity = _jumpForce;
    }

    public void Tick()
    {
        _verticalVelocity += _gravity * Time.deltaTime;
        _movement.Move(new Vector3(0, _verticalVelocity, 0));

        if (_verticalVelocity <= 0 && CheckIfGrounded())
        {
            _isGrounded = true;
        }
    }

    private bool CheckIfGrounded()
    {
        // Implement proper ground check here
        return true;
    }

    public void Exit()
    {
        _verticalVelocity = 0;
    }
}