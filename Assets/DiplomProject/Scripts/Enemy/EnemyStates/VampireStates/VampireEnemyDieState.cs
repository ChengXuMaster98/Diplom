using System.Collections;
using UnityEngine;

public class VampireEnemyDieState : IEnemyState
{
    private readonly IEnemyAnimator _animator;
    private readonly GameObject _enemyGO;

    public VampireEnemyDieState(IEnemyAnimator animator, GameObject enemyGO)
    {
        _animator = animator;
        _enemyGO = enemyGO;
    }

    public void Enter()
    {
        Debug.Log($"[Enemy] Умирает здесь?");
        _animator.PlayDie();

        if (_animator is EnemyAnimatorController controller)
        {
            controller.SetDeathEndCallback(OnDeathAnimationEnd);
        }
    }

    private void OnDeathAnimationEnd()
    {
        Debug.Log("[Enemy] Анимация смерти завершена, уничтожаем объект.");
        Object.Destroy(_enemyGO);
    }

    public void Exit() { }

    public void Tick() { }
}