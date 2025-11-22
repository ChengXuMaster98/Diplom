public interface IWeapon
{
    WeaponData Data { get; }

    void Attack(IEnemy enemy);
}