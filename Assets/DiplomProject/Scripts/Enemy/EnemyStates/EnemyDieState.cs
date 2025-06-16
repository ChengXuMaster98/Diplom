using UnityEngine;

public class EnemyDieState : IEnemyState
{
    private readonly IEnemyAnimator _animator;

    public EnemyDieState(IEnemyAnimator animator)
    {
        _animator = animator;
    }

    public void Enter()
    {
        Debug.Log($"[Enemy] Умирает здесь?");
        _animator.PlayDie();
    }

    public void Exit() { }

    public void Tick() { }
}