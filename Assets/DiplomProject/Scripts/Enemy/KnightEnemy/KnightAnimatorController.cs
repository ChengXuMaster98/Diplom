using System;
using UnityEngine;

public class KnightAnimatorController : MonoBehaviour, IKnightAnimator
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _transform;

    private Action _onAttackHit;
    private Action _onDeathAnimEnd;

    public Transform Transform => _transform;

    public void SetAttackHitCallback(Action onHit)
    {
        _onAttackHit = onHit;
    }

    public void SetDeathEndCallback(Action onDeathEnd)
    {
        _onDeathAnimEnd = onDeathEnd;
    }

    public void PlayIdle()
    {
        _animator.SetBool("IsMoving", false);
        _animator.SetBool("IsCircling", false);
    }

    public void PlayMove()
    {
        _animator.SetBool("IsMoving", true);
        _animator.SetBool("IsCircling", false);
    }

    public void PlayCircle()
    {
        _animator.SetBool("IsMoving", false);
        _animator.SetBool("IsCircling", true);
    }

    public void PlayAttack()
    {
        _animator.SetTrigger("Attack");
    }

    public void PlayRetreat()
    {
        _animator.SetTrigger("Retreat");
    }

    public void PlayDie()
    {
        _animator.SetTrigger("Die");
    }

    public void PlayImpact()
    {
        _animator.SetTrigger("Hit");
    }

    public void PlayStun()
    {
        _animator.SetTrigger("Stun");
    }

    public void LookAt(Vector3 position)
    {
        Vector3 dir = (position - _transform.position).normalized;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.001f)
            _transform.rotation = Quaternion.Slerp(
                _transform.rotation,
                Quaternion.LookRotation(dir),
                Time.deltaTime * 10f);
    }

    public bool IsPlayingAttack()
    {
        var state = _animator.GetCurrentAnimatorStateInfo(0);
        return state.IsName("Attack") && state.normalizedTime < 1f;
    }

    // вызывать из анимационного эвента
    public void DealDamage()
    {
        _onAttackHit?.Invoke();
    }

    // тоже через Animation Event
    public void OnDeathAnimationEnd()
    {
        _onDeathAnimEnd?.Invoke();
    }
}
