using System;
using UnityEngine;
using Zenject;

public class StaminaSystem : IStaminaSystem, ITickable
{
    private readonly StaminaConfig _config;
    public float CurrentStamina { get; private set; }
    public float MaxStamina => _config.MaxStamina * _upgradeService.StaminaMultiplier;

    public event Action<float> OnStaminaChanged;

    private readonly IUpgradeService _upgradeService;

    public StaminaSystem (StaminaConfig config, IUpgradeService upgradeService)
    {
        _config = config;
        CurrentStamina = _config.MaxStamina;
        _upgradeService = upgradeService;
    }



    public bool CanPerformBlock()
    => CurrentStamina >= _config.BlockCoast;

    public bool CanPerformAttack()
    => CurrentStamina >= _config.AttackCoast;

    public void SpendStaminaForBlock()
    {
        if (!CanPerformBlock())
            throw new
        InvalidOperationException("Not enough stamina to perrform block");


        CurrentStamina -= _config.BlockCoast;
        CurrentStamina = Mathf.Max(0, CurrentStamina);
        OnStaminaChanged?.Invoke(CurrentStamina);

        Debug.Log($"После траты стамины: {CurrentStamina}");
    }


    public void SpendStaminaForAttack()
    {
        Debug.Log($"Трата стамины. До: {CurrentStamina}");

        if (!CanPerformAttack())
            throw new
        InvalidOperationException("Not enough stamina to perform attack");

        CurrentStamina -= _config.AttackCoast;
        CurrentStamina = Mathf.Max(0, CurrentStamina);
        OnStaminaChanged?.Invoke(CurrentStamina);

        Debug.Log($"После траты стамины: {CurrentStamina}");
    }

    public void Tick()
    {
        if (CurrentStamina < MaxStamina)
        {
            //float before = CurrentStamina;

            float regenRate = _config.StaminaRegenPerSecond * _upgradeService.StaminaMultiplier;
            CurrentStamina += regenRate * Time.deltaTime;
            CurrentStamina = MathF.Min(CurrentStamina, MaxStamina);
            OnStaminaChanged?.Invoke(CurrentStamina);

            //Debug.Log($"Реген стамины: {before} -> {CurrentStamina}");
        }    
    }
}