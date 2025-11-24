using UnityEngine;

public class HammerWeapon : IWeapon
{
    public WeaponData Data { get; }
    private PlayerStats _stats;
    private IUpgradeService _upgrade;
    private WeaponSoundController _sound;

    public HammerWeapon(WeaponData data, PlayerStats stats, IUpgradeService upgrade, WeaponSoundController sound)
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

        // шанс стана
        if (Data.CanStun && enemy is IStunnable stunnable)
        {
            float chance = Random.value;
            if (chance <= Data.StunChance)
            {
                stunnable.ApplyStun(Data.StunDuration);
                Debug.Log("[Hammer] STUN applied");
            }
        }
    }

    public void SetTip(Transform tip)
    {
        
    }
}