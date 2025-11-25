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
    private readonly PlayerBlockState _blockState;

    private readonly IPauseService _pauseService;

    [Inject]
    public PlayerStateController(
        PlayerStateMachine stateMachine,
        Player player,
        CharacterMovementController movement,
        IPlayerStaminaConsumer staminaConsumer,
        AttackAnimationEventReceiver attackAnimationEventReceiver,
        DiContainer container,
        WeaponSoundController sound,
        PlayerWeaponInventory inventory,
        IPauseService pauseService)
    {
        _stateMachine = stateMachine;
        _pauseService = pauseService;


        _idleState = new PlayerIdleState(player.Animator);
        _moveState = new PlayerMoveState(player.Animator, movement);
        _jumpState = new PlayerJumpState(player.Animator, movement);
        _attackState = new PlayerAttackState(player.Animator, staminaConsumer, stateMachine, attackAnimationEventReceiver, sound, inventory);
        _blockState = new PlayerBlockState(player.Animator, staminaConsumer, stateMachine, sound, inventory);

       container.Unbind<IBlockStatusProvider>();
       container.Bind<IBlockStatusProvider>().FromInstance(_blockState).AsSingle();



        _stateMachine.SetState(_idleState);
    }

    public void Tick()
    {
        if (_pauseService.IsPaused)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _stateMachine.SetState(_jumpState);
            return; // Skip movement if jumping
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            _stateMachine.SetState(_attackState);
            return;
            // Skip other inputs if attacking
        }
        if(Input.GetMouseButton(1))
        {

            if (!(_stateMachine.CurrentState is PlayerBlockState))
                _stateMachine.SetState(_blockState);
            return;
        }

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
