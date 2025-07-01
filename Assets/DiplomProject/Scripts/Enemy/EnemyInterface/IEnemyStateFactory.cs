    public interface IEnemyStateFactory
{
    IEnemyState CreateIdleState();
    IEnemyState CreateChaseState();
    IEnemyState CreateAttackState();
    IEnemyState CreateDieState();
}