using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private static readonly int Attack = Animator.StringToHash("Attack");
    private readonly Animator _animator;
    private readonly AttackHitBox _attackHitBox;
    private readonly PlayerStateMachine _stateMachine;
    private readonly IPlayerStaminaConsumer _staminaConsumer;
    private readonly AttackAnimationEventReceiver _animationEventReceiver;


    private bool _attackStarted;
    private bool _attackComplete;
    public PlayerAttackState(Animator animator, AttackHitBox attackHitBox, IPlayerStaminaConsumer staminaConsumer, PlayerStateMachine stateMachine, AttackAnimationEventReceiver animationEventReceiver)
    {
        _animator = animator;
        _attackHitBox = attackHitBox;
        _staminaConsumer = staminaConsumer;
        _stateMachine = stateMachine;
        _animationEventReceiver = animationEventReceiver;
    }

    public void Enter()
    {
        if (!_staminaConsumer.CanAttack())
        {
            Debug.Log("Not enough stamina for attack");

            _animator.SetTrigger(Attack);

            _stateMachine.RevertToPreviousState();
            return;
        }
        
        _staminaConsumer.ConsumeStaminaForAttack();

        _animator.SetTrigger(Attack);
        _attackStarted = true;
        _attackComplete = false;

        _animationEventReceiver.OnAttackStart += AnimationAttackStart;
        _animationEventReceiver.OnAttackEnd += AnimationAttackEnd;

        Debug.Log("PlayerAttackState: Enter()");

    }

    public void Tick()
    {

    }

    public bool CanExit()
    {
        return _attackComplete;
    }

    public void Exit()
    {
        _attackStarted = false;
        _animator.SetBool("Attack", false);

        _animationEventReceiver.OnAttackStart -= AnimationAttackStart;
        _animationEventReceiver.OnAttackEnd -= AnimationAttackEnd;

        _attackHitBox.DisableHitbox();
    }

    public void AnimationAttackStart()
    {
        _attackHitBox.EnableHitbox();
    }

    public void AnimationAttackEnd()
    {
        _attackHitBox.DisableHitbox();
        _attackComplete = true;

        _stateMachine.RevertToPreviousState();

    }
}