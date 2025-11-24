    public interface IStateFactory
{
    IEnemyState CreateIdleState();
    IEnemyState CreateChaseState();
    IEnemyState CreateAttackState();
    IEnemyState CreateDieState();

    IEnemyState CreateGetDamageState();

    IEnemyState CreateStunState(float duration);

    IEnemyState CreatePatrolState();
}
