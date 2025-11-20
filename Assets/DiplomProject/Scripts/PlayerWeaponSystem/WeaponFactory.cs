using Zenject;

public class WeaponFactory
{
    public IWeapon Create(WeaponData data)
    {
        return data.Type switch
        {
            WeaponType.Axe => new AxeWeapon(data),
            WeaponType.Hammer => new HammerWeapon(data),
            WeaponType.Sword => new SwordWeapon(data),
            _ => null
        };
    }
}