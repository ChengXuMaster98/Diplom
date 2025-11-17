using System;
using Zenject;

public enum UpgradeType
{
    Health,
    Damage,
    Speed,
    Stamina
}

public class UpgradeService : IUpgradeService, IInitializable, IDisposable
{
    public event Action OnUpgradesChanged;

    public float HealthMultiplier { get; private set; } = 1f;
    public float DamageMultiplier { get; private set; } = 1f;
    public float SpeedMultiplier { get; private set; } = 1f;
    public float StaminaMultiplier { get; private set; } = 1f;

    public void Initialize() => ResetUpgrades();

    public void Dispose() => ResetUpgrades();

    public void ApplyUpgrade(UpgradeType type, float addValue)
    {
        switch (type)
        {
            case UpgradeType.Health: HealthMultiplier += addValue; break;
            case UpgradeType.Damage: DamageMultiplier += addValue; break;
            case UpgradeType.Speed: SpeedMultiplier += addValue; break;
            case UpgradeType.Stamina: StaminaMultiplier += addValue; break;
        }
        OnUpgradesChanged?.Invoke();
    }

    public void ResetUpgrades()
    {
        HealthMultiplier = 1f;
        DamageMultiplier = 1f;
        SpeedMultiplier = 1f;
        StaminaMultiplier = 1f;
        OnUpgradesChanged?.Invoke();
    }
}