using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private readonly Animator _animator;
    private readonly AttackHitBox _attackHitBox;
    private readonly PlayerStateMachine _stateMachine;
    private readonly IPlayerStaminaConsumer _staminaConsumer;
    private bool _attackStarted;
    public PlayerAttackState(Animator animator, AttackHitBox attackHitBox, IPlayerStaminaConsumer staminaConsumer, PlayerStateMachine stateMachine)
    {
        _animator = animator;
        _attackHitBox = attackHitBox;
        _staminaConsumer = staminaConsumer;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        if (!_staminaConsumer.CanAttack())
        {
            Debug.Log("Not enough stamina for attack");

            _animator.SetBool("Attack", false);

            _stateMachine.RevertToPreviousState();
            return;
        }
        
        _staminaConsumer.ConsumeStaminaForAttack();

        //Debug.Log($"Stamina after attack: {_staminaConsumer.CurrentStamina}");
        
        _attackStarted = true;
        _animator.SetBool("Attack", true);
        
        Debug.Log("PlayerAttackState: Enter()");

    }

    public void Tick()
    {

    }


    public void Exit()
    {
        _attackStarted = false;
        _animator.SetBool("Attack", false);

        _attackHitBox.DisableHitbox();
    }

    public void AnimationAttackStart()
    {
        _attackHitBox.EnableHitbox();
        Debug.Log("Хитбокс включен");
    }

    public void AnimationAttackEnd()
    {
        _attackHitBox.DisableHitbox();

        Debug.Log("Хитбокс выключен");

        if ( _attackStarted )
        {
            _stateMachine.SetState(new PlayerIdleState(_animator));
        }
    }
}