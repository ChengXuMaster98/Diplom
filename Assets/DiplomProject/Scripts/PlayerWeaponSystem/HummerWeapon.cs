using UnityEngine;

public class HammerWeapon : IWeapon
{
    public WeaponType Type => WeaponType.Hammer;
    public WeaponData Data { get; }

    private System.Random rnd = new();

    public HammerWeapon(WeaponData data)
    {
        Data = data;
    }

    public void Attack(IEnemy enemy)
    {
        enemy.TakeDamage(Mathf.RoundToInt(Data.BaseDamage));

        if (Data.CanStun && rnd.NextDouble() < Data.StunChance)
        {
            enemy.ApplyStun(Data.StunDuration);
        }
    }
}