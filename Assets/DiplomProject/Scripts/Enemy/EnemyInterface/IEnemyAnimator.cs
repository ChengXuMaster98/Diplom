using UnityEngine;

public interface IEnemyAnimator
{

    Transform Transform { get; }


    void Initialize();
    void PlayIdle();
    void PlayChase();
    void PlayAttack();
    void PlayDie();
    void LookAt(Vector3 position);
}