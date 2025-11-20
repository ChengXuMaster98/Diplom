public interface IEnemy
{
    void TakeDamage(int damage);
    void ApplyStun(float stunDuration);
    void ApplyDOT(float dOTDamagePerSecond, float dOTDuration);
}