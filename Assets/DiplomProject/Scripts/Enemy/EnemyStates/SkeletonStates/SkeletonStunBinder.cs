using Zenject;

public class SkeletonStunBinder : IInitializable
{
    private readonly Enemy _enemy;
    private readonly ISkeletonStateMachine _stateMachine;
    private readonly ISkeletonStateFactory _factory;

    public SkeletonStunBinder(
        Enemy enemy,
        ISkeletonStateMachine stateMachine,
        ISkeletonStateFactory factory)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
        _factory = factory;
    }

    public void Initialize()
    {
        _enemy.OnStunned += duration =>
        {
            if (_enemy.IsDead) return;

            var stunState = _factory.CreateStunState(duration);
            _stateMachine.SetState(stunState);
        };
    }
}