public interface IKnightStateFactory : IStateFactory
{
    IEnemyState CreateCircleState();
    IEnemyState CreateRetreatState();
}