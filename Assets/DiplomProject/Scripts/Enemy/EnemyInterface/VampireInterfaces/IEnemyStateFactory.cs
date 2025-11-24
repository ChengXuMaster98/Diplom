    public interface IEnemyStateFactory
{
    IEnemyState CreateIdleState();
    IEnemyState CreateChaseState();
    IEnemyState CreateAttackState();
    IEnemyState CreateDieState();

    IEnemyState CreateGetDamageState();

    IEnemyState CreateStunState(float duration);
}