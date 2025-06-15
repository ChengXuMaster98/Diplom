using Zenject;

public class EnemyIdleState : IEnemyState
{
    private readonly IEnemyAnimator _animator;


    public EnemyIdleState(IEnemyAnimator animator)
    {
        _animator = animator;
    }

    public void Enter()
    {
        _animator.PlayIdle();
    }
    public void Tick()
    {

    }
    public void Exit() { }
}
