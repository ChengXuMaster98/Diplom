using Zenject;

public class WeaponFactory
{
    private readonly PlayerStats _stats;
    private readonly IUpgradeService _upgrade;
    private readonly WeaponSoundController _sound;

    public WeaponFactory(PlayerStats stats, IUpgradeService upgrade, WeaponSoundController sound)
    {
        _stats = stats;
        _upgrade = upgrade;
        _sound = sound;
    }
    public IWeapon Create(WeaponData data)
    {
        return data.Type switch
        {
            WeaponType.Axe => new AxeWeapon(data, _stats, _upgrade, _sound),
            WeaponType.Hammer => new HammerWeapon(data, _stats, _upgrade, _sound),
            WeaponType.Sword => new SwordWeapon(data, _stats, _upgrade, _sound),
            _ => null
        };
    }
}