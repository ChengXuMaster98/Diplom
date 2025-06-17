using UnityEngine;
using Zenject;

public class PlayerStateController : ITickable
{
    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";
    
    private readonly PlayerStateMachine _stateMachine;
    private readonly PlayerIdleState _idleState;
    private readonly PlayerMoveState _moveState;
    private readonly PlayerJumpState _jumpState;
    private readonly PlayerAttackState _attackState;

    [Inject]
    public PlayerStateController(
        PlayerStateMachine stateMachine,
        Player player,
        CharacterMovementController movement,
        AttackHitBox attackHitBox,
        IPlayerStaminaConsumer staminaConsumer)
    {
        _stateMachine = stateMachine;

        // ������ ��������� ��������, ��� ������������� Resolve()
        _idleState = new PlayerIdleState(player.Animator);
        _moveState = new PlayerMoveState(player.Animator, movement);
        _jumpState = new PlayerJumpState(player.Animator, movement);
        _attackState = new PlayerAttackState(player.Animator, attackHitBox, staminaConsumer, stateMachine);

        // �� ��������� ������ � ��������� Idle
        _stateMachine.SetState(_idleState);
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _stateMachine.SetState(_jumpState);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            _stateMachine.SetState(_attackState);
        }
        _stateMachine?.Tick();
        return;
        var moveX = Input.GetAxis(HORIZONTAL_AXIS);
        var moveZ = Input.GetAxis(VERTICAL_AXIS);
        var input = new Vector2(moveX, moveZ);

        if (input.magnitude == 0)
        {
            _stateMachine.SetState(_idleState);
        }
        else
        {
            _stateMachine.SetState(_moveState);
        }
        
        _stateMachine?.Tick();
    }
}
