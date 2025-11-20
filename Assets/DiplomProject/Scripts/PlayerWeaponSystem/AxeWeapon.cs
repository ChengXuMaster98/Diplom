using UnityEngine;

public class AxeWeapon : IWeapon
{
    public WeaponType Type => WeaponType.Axe;
    public WeaponData Data { get; }

    public AxeWeapon(WeaponData data)
    {
        Data = data;
    }

    public void Attack(IEnemy enemy)
    {
        enemy.TakeDamage(Mathf.RoundToInt(Data.BaseDamage));
    }
}