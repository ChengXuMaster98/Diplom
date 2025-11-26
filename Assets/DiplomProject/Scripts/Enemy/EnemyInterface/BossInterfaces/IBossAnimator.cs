using System;
using UnityEngine;

public interface IBossAnimator
{
    Transform Transform { get; }
    void PlayIdle();
    void PlayChase();
    void PlayAttack();
    void PlayDie();

    void PlayPatrol();
    void PlayImpact();

    void PlayStun();
    void LookAt(Vector3 position);
    bool IsPlayingAttackAnimation();

    void SetAttackHitCallback(Action onHit);
}