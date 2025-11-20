using System;

public interface IUpgradeService
{
    event Action OnUpgradesChanged;

    float HealthMultiplier { get; }
    float DamageMultiplier { get; }
    float SpeedMultiplier { get; }
    float StaminaMultiplier { get; }

    void ApplyUpgrade(UpgradeType type, float addValue);
    void ResetUpgrades();

    void SetMultipliers(float health, float damage, float speed, float stamina);
}