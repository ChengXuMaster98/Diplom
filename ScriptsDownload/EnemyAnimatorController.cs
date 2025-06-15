using UnityEngine;

public class EnemyAnimatorController
{
    private readonly Animator _animator;

    public EnemyAnimatorController(Animator animator)
    {
        _animator = animator;
    }

    public void PlayIdle() => _animator.SetTrigger("Idle");
    public void StopIdle() => _animator.ResetTrigger("Idle");

    public void PlayPatrol() => _animator.SetTrigger("Patrol");
    public void StopPatrol() => _animator.ResetTrigger("Patrol");

    public void PlayChase() => _animator.SetTrigger("Chase");
    public void StopChase() => _animator.ResetTrigger("Chase");

    public void PlayAttack() => _animator.SetTrigger("Attack");

    public void PlayDie() => _animator.SetTrigger("Die");
}