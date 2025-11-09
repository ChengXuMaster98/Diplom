
public interface IEnemyStateMachine
{
    void Initialize(IEnemyState idle, IEnemyState chase, IEnemyState attack, IEnemyState die);
    void SetState(IEnemyState newState);
    void Tick();
    void SetToDieState();
    void Initialize(IEnemyState idleState);
}