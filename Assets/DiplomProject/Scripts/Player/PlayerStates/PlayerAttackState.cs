using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private static readonly int Attack = Animator.StringToHash("Attack");
    private readonly Animator _animator;
    private readonly PlayerStateMachine _stateMachine;
    private readonly IPlayerStaminaConsumer _staminaConsumer;
    private readonly AttackAnimationEventReceiver _animationEventReceiver;
    private readonly WeaponSoundController _sound;
    private readonly PlayerWeaponInventory _inventory;

    private bool _attackComplete;

    public PlayerAttackState(Animator animator, IPlayerStaminaConsumer staminaConsumer, PlayerStateMachine stateMachine, AttackAnimationEventReceiver animationEventReceiver, WeaponSoundController sound, PlayerWeaponInventory inventory)
    {
        _animator = animator;
        _staminaConsumer = staminaConsumer;
        _stateMachine = stateMachine;
        _animationEventReceiver = animationEventReceiver;
        _sound = sound;
        _inventory = inventory;
    }

    public void Enter()
    {

        var weapon = _inventory.GetActiveWeapon();
        if (weapon == null)
        {
            Debug.Log("[Attack] Нет оружия — атака невозможна");
            _stateMachine.RevertToPreviousState();
            return;
        }

        //_sound.PlayLightAttack(weapon.Data.SoundData);

        if (!_staminaConsumer.CanAttack())
        {
            Debug.Log("Not enough stamina for attack");

            //_animator.SetTrigger(Attack);

            _stateMachine.RevertToPreviousState();
            return;
        }


        _staminaConsumer.ConsumeStaminaForAttack();

        _animator.SetTrigger(Attack);
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
        _animator.SetBool(Attack, false);

        _animationEventReceiver.OnAttackStart -= AnimationAttackStart;
        _animationEventReceiver.OnAttackEnd -= AnimationAttackEnd;
    }

    public void AnimationAttackStart()
    {
        //_sound.PlayLightAttack();

    }

    public void AnimationAttackEnd()
    {
        _attackComplete = true;

        _stateMachine.RevertToPreviousState();

    }
}