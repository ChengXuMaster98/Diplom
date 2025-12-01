using System;
using UnityEngine;

public interface IKnightAnimator
{
    Transform Transform { get; }

    void PlayIdle();
    void PlayMove();        // движение вперёд
    void PlayCircle(float side);  // кружение вокруг (лево/право)
    void PlayRetreat();

    void PlayAttack();
    void PlayDie();
    void PlayImpact();
    void PlayStun();

    void SetRootMotion(bool enabled);

    void LookAt(Vector3 position);
    bool IsPlayingAttack();

    void SetAttackHitCallback(Action onHit);
    void SetDeathEndCallback(Action onDeathEnd);
}