using Zenject;

public class BossStunBinder : IInitializable
{
    private readonly Enemy _enemy;
    private readonly IBossStateMachine _stateMachine;
    private readonly IBossStateFactory _factory;

    public BossStunBinder(
        Enemy enemy,
        IBossStateMachine stateMachine,
        IBossStateFactory factory)
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