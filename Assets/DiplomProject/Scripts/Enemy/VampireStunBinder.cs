using Zenject;

public class VampireStunBinder : IInitializable
{
    private readonly Enemy _enemy;
    private readonly IEnemyStateMachine _stateMachine;
    private readonly IEnemyStateFactory _factory;

    public VampireStunBinder(
        Enemy enemy,
        IEnemyStateMachine stateMachine,
        IEnemyStateFactory factory)
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