using UnityEngine;

public class BossDieState : IEnemyState
{
    private readonly IBossAnimator _animator;
    private readonly GameObject _enemyGO;
    private readonly GameWonUI _gameWonUI;

    public BossDieState(IBossAnimator animator, GameObject enemyGO, GameWonUI gameWonUI)
    {
        _animator = animator;
        _enemyGO = enemyGO;
        _gameWonUI = gameWonUI;
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

        _gameWonUI.Show();
    }

    public void Tick() { }

    public void Exit() { }
}
