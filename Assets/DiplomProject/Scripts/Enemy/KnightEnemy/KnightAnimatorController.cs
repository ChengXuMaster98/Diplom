using System;
using UnityEngine;

public class KnightAnimatorController : MonoBehaviour, IKnightAnimator
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _model;   // корневая кость персонажа / объект модели

    private Action _onAttackHit;
    private Action _onDeathAnimationEnd;

    // Animator parameters
    private static readonly int MoveXHash = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash = Animator.StringToHash("MoveY");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int AttackIndexHash = Animator.StringToHash("AttackIndex");
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int StunHash = Animator.StringToHash("Stun");
    private static readonly int DieHash = Animator.StringToHash("Die");

    public Transform Transform => _model;

    // ---------- MOVEMENT ----------
    public void PlayIdle()
    {
        _animator.SetFloat(MoveXHash, 0f);
        _animator.SetFloat(MoveYHash, 0f);
    }

    public void PlayMove()      // бег вперёд
    {
        _animator.SetFloat(MoveXHash, 0f);
        _animator.SetFloat(MoveYHash, 1f);
    }

    public void PlayCircle(float side)
    {
        // side = -1 (влево) или 1 (вправо)
        _animator.SetFloat(MoveXHash, side * 0.7f);
        _animator.SetFloat(MoveYHash, 0.7f);
    }

    public void PlayRetreat()
    {
        _animator.SetFloat(MoveXHash, 0f);
        _animator.SetFloat(MoveYHash, -1f);
    }

    // ---------- ATTACK ----------
    public void PlayAttack()
    {
        int index = UnityEngine.Random.Range(0, 3); // 0,1,2
        _animator.SetInteger(AttackIndexHash, index);
        _animator.SetTrigger(AttackHash);
    }

    public bool IsPlayingAttack()
    {
        var s = _animator.GetCurrentAnimatorStateInfo(0);

        // поставь сюда реальные имена твоих attack-клипов
        return s.IsName("BaseAttack") ||
               s.IsName("Attack2") ||
               s.IsName("Attack3");
    }

    public void SetAttackHitCallback(Action onHit)
    {
        _onAttackHit = onHit;
    }

    public void SetDeathEndCallback(Action onDeathEnd)
    {
        _onDeathAnimationEnd = onDeathEnd;
    }

    // Animation Event
    public void DealDamage()
    {
        _onAttackHit?.Invoke();
    }

    // Animation Event
    public void OnDeathAnimationEnd()
    {
        _onDeathAnimationEnd?.Invoke();
    }

    public void SetRootMotion(bool enabled)
    {
        _animator.applyRootMotion = enabled;
    }

    // ---------- HIT / STUN / DIE ----------
    public void PlayImpact()
    {
        _animator.SetTrigger(HitHash);
    }

    public void PlayStun()
    {
        _animator.SetTrigger(StunHash);
    }

    public void PlayDie()
    {
        _animator.SetTrigger(DieHash);
    }

    // ---------- LOOK AT ----------
    public void LookAt(Vector3 worldPos)
    {
        Vector3 dir = worldPos - _model.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRot = Quaternion.LookRotation(dir.normalized);
        _model.rotation = Quaternion.Slerp(_model.rotation, targetRot, Time.deltaTime * 8f);
    }
}