using UnityEngine;

public class SwordWeapon : IWeapon
{
    public WeaponData Data { get; }
    private PlayerStats _stats;
    private IUpgradeService _upgrade;
    private WeaponSoundController _sound;

    public SwordWeapon(WeaponData data, PlayerStats stats, IUpgradeService upgrade, WeaponSoundController sound)
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


        // шанс DOT
        if (Data.CanElectroDOT && enemy is IDamageOverTime dot)
        {
            float chance = Random.value;
            if (chance <= Data.DOTChance)
            {
                dot.ApplyDoT(Data.DOTDamagePerSecond, Data.DOTDuration);
                Debug.Log("[Sword] ELECTRO DOT applied");
            }
        }
    }
}