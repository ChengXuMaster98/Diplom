using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class SkeletonDieState : IEnemyState
{
    private readonly ISkeletonAnimator _animator;
    private readonly GameObject _enemyGO;

    public SkeletonDieState(ISkeletonAnimator animator, GameObject enemyGO)
    {
        _animator = animator;
        _enemyGO = enemyGO;
    }

    public void Enter()
    {
        _animator.PlayDie();
        
        if (_animator is SkeletonAnimatorController controller)
        {
            controller.SetDeathEndCallback(OnDeathAnimationEnd);
        }
    }

    private void OnDeathAnimationEnd()
    {
        Debug.Log("[Enemy] Анимация смерти завершена, уничтожаем объект.");
        Object.Destroy(_enemyGO);
    }

    public void Tick() { }

    public void Exit() { }
}
