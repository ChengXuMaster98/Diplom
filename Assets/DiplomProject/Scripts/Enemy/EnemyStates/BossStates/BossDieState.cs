using UnityEngine;

public class BossDieState : IEnemyState
{
    private readonly IBossAnimator _animator;
    private readonly GameObject _enemyGO;

    public BossDieState(IBossAnimator animator, GameObject enemyGO)
    {
        _animator = animator;
        _enemyGO = enemyGO;
    }

    public void Enter()
    {
        _animator.PlayDie();

        if (_animator is BossAnimatorController controller)
        {
            controller.SetDeathEndCallback(OnDeathAnimationEnd);
        }
    }

    private void OnDeathAnimationEnd()
    {
        Debug.Log("[Enemy] Destroy target = " + _enemyGO.name);

        Debug.Log("[Enemy] Анимация смерти завершена, уничтожаем объект.");
        Object.Destroy(_enemyGO);

        Debug.Log("[Enemy] After Destroy, still exists? " + (_enemyGO != null));
    }

    public void Tick() { }

    public void Exit() { }
}
