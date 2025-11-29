using UnityEngine;

public class KnightDieState : IEnemyState
{
    private readonly IKnightAnimator _animator;
    private readonly GameObject _enemyGO;

    public KnightDieState(IKnightAnimator animator, GameObject enemyGO)
    {
        _animator = animator;
        _enemyGO = enemyGO;
    }

    public void Enter()
    {
        _animator.PlayDie();
        if (_animator is KnightAnimatorController k)
            k.SetDeathEndCallback(OnDeathAnimationEnd);
    }

    private void OnDeathAnimationEnd()
    {
        Object.Destroy(_enemyGO);
    }

    public void Tick() { }
    public void Exit() { }
}