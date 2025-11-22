using System;
using UnityEngine;

public class SkeletonAnimatorController : MonoBehaviour, ISkeletonAnimator
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _transform;

    private Action _onAttackHit;
    private Action _onDeathAnimationEnd;

    public Transform Transform => _transform;

    public void SetAttackHitCallback(Action onHit)
    {
        _onAttackHit = onHit;
    }

    public void SetDeathEndCallback(Action onDeathEnd) // 👈 вызывать из DieState
    {
        _onDeathAnimationEnd = onDeathEnd;
    }

    public void DealDamage()
    {
        Debug.Log("[Animator] Attack animation event triggered");
        _onAttackHit?.Invoke();
    }

    public void OnDeathAnimationEnd()
    {
        _onDeathAnimationEnd?.Invoke();
    }

    public void LookAt(Vector3 position)
    {
        Vector3 direction = (position - _transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            _transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public bool IsPlayingAttackAnimation()
    {
        return
        _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
        _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
    }
    public void PlayIdle()
    {
        Debug.Log("PlayIdle called");
        _animator.SetBool("IsChasing", false);
        //_animator.SetBool("IsIdle", true);
        //_animator.SetBool("IsChasing", false);
    }

    public void PlayChase()
    {
        Debug.Log("PlayChase called");
        _animator.SetBool("IsChasing", true);
    }

    public void PlayAttack()
    {
        _animator.SetBool("IsChasing", false);
        _animator.SetTrigger("Attacking");
    }
    public void PlayDie()
    {
        _animator.SetTrigger("Dying");
    }

    public void PlayImpact()
    {
        _animator.SetTrigger("GetDamage");
    }

    public void PlayFly()
    {
        _animator.SetTrigger("Fly");
    }

    public void PlayStun()
    {
        _animator.SetTrigger("Stun");
    }
}
