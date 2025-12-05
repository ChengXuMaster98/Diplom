using UnityEngine;

public class PlayerStaminaAdapter : IPlayerStaminaConsumer
{
    private readonly IStaminaSystem _staminaSystem;

    public PlayerStaminaAdapter(IStaminaSystem staminaSystem)
    {
        _staminaSystem = staminaSystem;
        Debug.Log("Стамина адаптер создан");
    }

    public bool CanDash()
    {
        bool canDash = _staminaSystem.CanPerformDash();

        Debug.Log($"CanDash: {canDash}, Current Stamina: {_staminaSystem.CurrentStamina}");

        return canDash;
    }

    public void ConsumeStaminaForDash()
    {
        Debug.Log($"Consuming stamina for dash. Before: {_staminaSystem.CurrentStamina}");

        _staminaSystem.SpendStaminaForDash();

        Debug.Log($"After Consumption: {_staminaSystem.CurrentStamina}");
    }

    public bool CanBlock()
    {
        bool canBlock = _staminaSystem.CanPerformBlock();

        Debug.Log($"CanBlock: {canBlock}, Current Stamina: {_staminaSystem.CurrentStamina}");

        return canBlock;
    }

    public void ConsumeStaminaForBlock()
    {
        Debug.Log($"Consuming stamina for block. Before: {_staminaSystem.CurrentStamina}");

        _staminaSystem.SpendStaminaForBlock();

        Debug.Log($"After Consumption: {_staminaSystem.CurrentStamina}");
    }

    public bool CanAttack()
    {
        bool canAttack = _staminaSystem.CanPerformAttack();

        Debug.Log($"CanAttack: {canAttack}, Current Stamina: {_staminaSystem.CurrentStamina}");

        return canAttack;
    }

    public void ConsumeStaminaForAttack()
    {
        Debug.Log($"Consuming stamina for attack. Before: {_staminaSystem.CurrentStamina}");

        _staminaSystem.SpendStaminaForAttack();

        Debug.Log($"After Consumption: {_staminaSystem.CurrentStamina}");
    }
}
