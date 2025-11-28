using Zenject;

public class SkinnyOrkStunBinder : IInitializable
{
    private readonly Enemy _enemy;
    private readonly ISkinnyOrkStateMachine _stateMachine;
    private readonly ISkinnyOrkStateFactory _factory;

    public SkinnyOrkStunBinder(
        Enemy enemy,
        ISkinnyOrkStateMachine stateMachine,
        ISkinnyOrkStateFactory factory)
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