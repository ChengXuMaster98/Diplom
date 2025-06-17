using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStaminaAdapter : IPlayerStaminaConsumer
{
    private readonly IStaminaSystem _staminaSystem;

    public PlayerStaminaAdapter(IStaminaSystem staminaSystem)
    {
        _staminaSystem = staminaSystem;
        Debug.Log("Стамина адаптер создан");
    }

    public bool CanAttack()
    {
        bool canAttack = _staminaSystem.CanPerformAttack();

        Debug.Log($"CanAttack: {canAttack}, Current Stamina: {_staminaSystem.CurrentStamina}");

        return canAttack;
    }

    public void ConsumeStaminaForAttack()
    {
        Debug.Log($"Consuming stamina. Before: {_staminaSystem.CurrentStamina}");

        _staminaSystem.SpendStaminaForAttack();

        Debug.Log($"After Consumption: {_staminaSystem.CurrentStamina}");
    }
}
