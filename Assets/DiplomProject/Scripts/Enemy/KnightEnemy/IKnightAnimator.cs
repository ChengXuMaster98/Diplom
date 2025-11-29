using System;
using UnityEngine;

public interface IKnightAnimator
{
    Transform Transform { get; }

    void PlayIdle();
    void PlayMove();        // общее движение вперёд
    void PlayCircle();      // кружение/бег вокруг
    void PlayAttack();
    void PlayRetreat();
    void PlayDie();
    void PlayImpact();
    void PlayStun();

    void LookAt(Vector3 position);
    bool IsPlayingAttack();

    void SetAttackHitCallback(Action onHit);
    void SetDeathEndCallback(Action onDeathEnd);
}