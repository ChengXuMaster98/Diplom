using System;
using UnityEngine;

public interface ISkeletonAnimator
{
    Transform Transform { get; }
    void PlayIdle();
    void PlayChase();
    void PlayAttack();
    void PlayDie();

    void PlayFly();
    void PlayImpact();

    void PlayStun();
    void LookAt(Vector3 position);
    bool IsPlayingAttackAnimation();

    void SetAttackHitCallback(Action onHit);
}