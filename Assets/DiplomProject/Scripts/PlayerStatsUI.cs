using UnityEngine;
using TMPro;
using Zenject;

public class PlayerStatsUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject rootPanel;

    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text staminaText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text moveSpeedText;
    [SerializeField] private TMP_Text upgradesText;

    private PlayerHealth _health;
    private IStaminaSystem _stamina;
    private PlayerStats _stats;
    private IUpgradeService _upgrades;

    [Inject]
    public void Construct(
        PlayerHealth health,
        IStaminaSystem stamina,
        PlayerStats stats,
        IUpgradeService upgrades)
    {
        _health = health;
        _stamina = stamina;
        _stats = stats;
        _upgrades = upgrades;
    }

    private void Update()
    {
        UpdateStats();
    }

    private void UpdateStats()
    {
        // БАЗОВЫЕ статы (из ScriptableObject)
        int baseHealth = _stats.MaxHealth;
        float baseDamage = _stats.attackDamage;
        float baseSpeed = _stats.MoveSpeed;
        float baseStamina = _stamina.MaxStamina / _upgrades.StaminaMultiplier;

        // ИТОГОВЫЕ статы после апгрейдов
        int finalHealth = _health.MaxHealth;
        float finalDamage = baseDamage * _upgrades.DamageMultiplier;
        float finalSpeed = baseSpeed * _upgrades.SpeedMultiplier;
        float finalStamina = _stamina.MaxStamina;

        // UI отображение
        healthText.text =
            $"Health: {finalHealth}  (base: {baseHealth}, +{finalHealth - baseHealth})";

        staminaText.text =
            $"Stamina: {finalStamina:F1}  (base: {baseStamina:F1}, +{finalStamina - baseStamina:F1})";

        damageText.text =
            $"Damage: {finalDamage:F1}  (base: {baseDamage}, x{_upgrades.DamageMultiplier:F2})";

        moveSpeedText.text =
            $"Move Speed: {finalSpeed:F1}  (base: {baseSpeed}, x{_upgrades.SpeedMultiplier:F2})";

        upgradesText.text =
            $"UPGRADES MULTIPLIERS:\n" +
            $"Health x{_upgrades.HealthMultiplier:F2}\n" +
            $"Damage x{_upgrades.DamageMultiplier:F2}\n" +
            $"Speed x{_upgrades.SpeedMultiplier:F2}\n" +
            $"Stamina x{_upgrades.StaminaMultiplier:F2}";
    }
}