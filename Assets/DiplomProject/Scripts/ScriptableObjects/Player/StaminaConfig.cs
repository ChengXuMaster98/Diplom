using UnityEngine;

[CreateAssetMenu(fileName = "StaminaConfig", menuName = "Player/StaminaConfig")]
public class StaminaConfig: ScriptableObject
{
    [SerializeField] private float _maxStamina = 100f;
    [SerializeField] private float _staminaRegenPerSecond = 20f;
    [SerializeField] private float _attackCoast = 30f;

    public float MaxStamina => _maxStamina;
    public float StaminaRegenPerSecond => _staminaRegenPerSecond;
    public float AttackCoast => _attackCoast;
}
