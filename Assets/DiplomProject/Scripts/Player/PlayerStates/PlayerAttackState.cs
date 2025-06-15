using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private readonly Animator _animator;
    private readonly AttackHitBox _attackHitBox;
    public PlayerAttackState(Animator animator, AttackHitBox attackHitBox)
    {
        _animator = animator;
        _attackHitBox = attackHitBox;
    }

    public void Enter()
    {
        Debug.Log("PlayerAttackState: Enter()");
        _animator.SetBool("Attack", true);
    }

    public void Tick()
    {

    }


    public void Exit()
    {
        _attackHitBox.DisableHitbox();
    }

    public void AnimationAttackStart()
    {
        _attackHitBox.EnableHitbox();
    }

    public void AnimationAttackEnd()
    {
        _attackHitBox.DisableHitbox();
    }
}