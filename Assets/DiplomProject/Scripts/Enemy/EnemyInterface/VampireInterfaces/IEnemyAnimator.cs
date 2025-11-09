using System;
using UnityEngine;

public interface IEnemyAnimator
{
    Transform Transform { get; }
    void PlayIdle();
    void PlayChase();
    void PlayAttack();
    void PlayDie();
    void PlayImpact();
    void LookAt(Vector3 position);
    bool IsPlayingAttackAnimation();

    void SetAttackHitCallback(Action onHit);
}