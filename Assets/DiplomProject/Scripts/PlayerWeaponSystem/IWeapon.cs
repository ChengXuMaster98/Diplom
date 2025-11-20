public interface IWeapon
{
    WeaponType Type { get; }
    WeaponData Data { get; }

    void Attack(IEnemy enemy);
}