using UnityEngine;

public class AxeWeapon : IWeapon
{
    public WeaponData Data { get; }
    private PlayerStats _stats;
    private IUpgradeService _upgrade;
    private WeaponSoundController _sound;

    public AxeWeapon(WeaponData data, PlayerStats stats, IUpgradeService upgrade, WeaponSoundController sound)
    {
        Data = data;
        _stats = stats;
        _upgrade = upgrade;
        _sound = sound;
    }

    public void Attack(IEnemy enemy)
    {
        int damage = Mathf.RoundToInt(
            (_stats.attackDamage + Data.BaseDamage) * _upgrade.DamageMultiplier
        );

        enemy.TakeDamage(damage);
    }
}