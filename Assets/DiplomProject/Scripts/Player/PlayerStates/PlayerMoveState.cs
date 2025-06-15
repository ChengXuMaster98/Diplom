using UnityEngine;

public class PlayerMoveState : IPlayerState
{
    private readonly Animator _animator;
    private readonly CharacterMovementController _movement;

    public PlayerMoveState(Animator animator, CharacterMovementController movement)
    {
        _animator = animator;
        _movement = movement;
    }

    public void Enter()
    {
        _animator.SetBool("isWalking", true);
    }

    public void Tick()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector2 input = new Vector2(moveX, moveZ);

        _movement.Move(input);

        if (input.magnitude == 0)
        {
            _animator.SetBool("isWalking", false);
        }
    }

    public void Exit()
    {
        _animator.SetBool("isWalking", false);
        _movement.Move(Vector3.zero);
    }
}
