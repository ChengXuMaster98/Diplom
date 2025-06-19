using UnityEngine;

public class PlayerMoveState : IPlayerState
{
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    
    private readonly Animator _animator;
    private readonly CharacterMovementController _movement;
    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";

    public PlayerMoveState(Animator animator, CharacterMovementController movement)
    {
        _animator = animator;
        _movement = movement;
    }

    public void Enter()
    {
        _animator.SetBool(IsWalking, true);
        return;
    }

    public void Tick()
    {
     
        float moveX = Input.GetAxis(HORIZONTAL_AXIS);
        float moveZ = Input.GetAxis(VERTICAL_AXIS);
        Vector2 input = new Vector2(moveX, moveZ);

        _movement.Move(input);
        _animator.SetBool(IsWalking, input.magnitude != 0);
    }

    public void Exit()
    {
        _animator.SetBool(IsWalking, false);
        _movement.Move(Vector3.zero);
    }

    public bool CanExit()
    {
        // Can exit if not moving (transition to idle will be handled by controller)
        return true;
    }
}
