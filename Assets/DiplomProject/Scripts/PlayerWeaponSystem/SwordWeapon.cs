using UnityEngine;

public class SwordWeapon : IWeapon
{
    public WeaponType Type => WeaponType.Sword;
    public WeaponData Data { get; }

    private System.Random rnd = new();

    public SwordWeapon(WeaponData data)
    {
        Data = data;
    }

    public void Attack(IEnemy enemy)
    {
        enemy.TakeDamage(Mathf.RoundToInt(Data.BaseDamage));

        if (Data.CanElectroDOT && rnd.NextDouble() < Data.DOTChance)
        {
            enemy.ApplyDOT(Data.DOTDamagePerSecond, Data.DOTDuration);
        }
    }
}