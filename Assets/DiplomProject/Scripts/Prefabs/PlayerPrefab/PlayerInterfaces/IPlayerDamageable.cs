using Unity.VisualScripting;

public interface IPlayerDamageable
{
    void TakeDamage(int amount);

    bool IsDead { get; }
}