using UnityEngine;
using Zenject;

public class PlayerStateController : ITickable
{
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
        AttackHitBox attackHitBox)
    {
        _stateMachine = stateMachine;

        // Создаём состояния напрямую, без использования Resolve()
        _idleState = new PlayerIdleState(player.Animator);
        _moveState = new PlayerMoveState(player.Animator, movement);
        _jumpState = new PlayerJumpState(player.Animator, movement);
        _attackState = new PlayerAttackState(player.Animator, attackHitBox);

        // По умолчанию входим в состояние Idle
        _stateMachine.SetState(_idleState);
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _stateMachine.SetState(_jumpState);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            _stateMachine.SetState(_attackState);
        }
        else
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            if (moveX != 0 || moveZ != 0)
            {
                _stateMachine.SetState(_moveState);
            }
            else
            {
                _stateMachine.SetState(_idleState);
            }
        }

        _stateMachine.Tick();
    }
}
