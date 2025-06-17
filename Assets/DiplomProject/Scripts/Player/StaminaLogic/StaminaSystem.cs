using System;
using UnityEngine;
using Zenject;

public class StaminaSystem : IStaminaSystem, ITickable
{
    private readonly StaminaConfig _config;
    public float CurrentStamina { get; private set; }
    public float MaxStamina => _config.MaxStamina;

    public StaminaSystem (StaminaConfig config)
    {
        _config = config;
        CurrentStamina = _config.MaxStamina;
    }

    public bool CanPerformAttack()
    => CurrentStamina >= _config.AttackCoast;

    public void SpendStaminaForAttack()
    {
        Debug.Log($"Трата стамины. До: {CurrentStamina}");

        if (!CanPerformAttack())
            throw new
        InvalidOperationException("Not enough stamina to perform attack");

        CurrentStamina -= _config.AttackCoast;
        CurrentStamina = Mathf.Max(0, CurrentStamina);

        Debug.Log($"После траты стамины: {CurrentStamina}");
    }

    public void Tick()
    {
        if (CurrentStamina < MaxStamina)
        {
            float before = CurrentStamina;

            CurrentStamina += _config.StaminaRegenPerSecond * Time.deltaTime;
            CurrentStamina = MathF.Min(CurrentStamina, MaxStamina);

            //Debug.Log($"Реген стамины: {before} -> {CurrentStamina}");
        }    
    }
}