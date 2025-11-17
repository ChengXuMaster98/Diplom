using UnityEngine;

public class PlayerBlockState : IPlayerState, IBlockStatusProvider
{
    private static readonly int Block = Animator.StringToHash("Block");

    private readonly Animator _animator;
    private readonly PlayerStateMachine _stateMachine;
    private readonly IPlayerStaminaConsumer _staminaConsumer;

    private bool _isBlocking;

    public bool IsBlocking => _isBlocking;

    public PlayerBlockState(Animator animator, IPlayerStaminaConsumer staminaConsumer, PlayerStateMachine stateMachine)
    {
        _animator = animator;
        _staminaConsumer = staminaConsumer;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        if (!_staminaConsumer.CanBlock())
        {
            Debug.Log("Not enough stamina to block");
            _stateMachine.RevertToPreviousState();
            return;
        }

        _isBlocking = true;
        _animator.SetBool(Block, true);
        Debug.Log("[Block] Started");
    }

    public void Tick()
    {
        if (!Input.GetMouseButton(1))
        {
            Debug.Log("[Block] Released");
            _stateMachine.RevertToPreviousState();
        }
    }

    public bool CanExit() => true;

    public void Exit()
    {
        _animator.SetBool(Block, false);
        _isBlocking = false;
        Debug.Log("[Block] Stopped");
    }
}