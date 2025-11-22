
    public interface IStateMachine
    {
    //void Initialize(IEnemyState idle, IEnemyState chase, IEnemyState attack, IEnemyState die);

    IEnemyState CurrentState { get; }

    void SetState(IEnemyState newState);
    void Tick();
    void SetToDieState();

    void RevertToPreviousState();
    void Initialize(IEnemyState idleState);
}
