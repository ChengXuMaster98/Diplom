using System;
using UnityEngine;
using Zenject;

public class EnemyAnimatorController: MonoBehaviour, IEnemyAnimator
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _transform;

    private Action _onAttackHit;

    public Transform Transform => _transform;

    public void SetAttackHitCallback(Action onHit)
    {
        _onAttackHit = onHit;
    }

    public void DealDamage()
    {
        _onAttackHit?.Invoke();
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
        _animator.SetTrigger("D");
    }
}