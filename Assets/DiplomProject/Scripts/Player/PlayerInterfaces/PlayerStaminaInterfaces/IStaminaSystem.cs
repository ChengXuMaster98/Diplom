using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IStaminaSystem
{
    float CurrentStamina { get; }
    float MaxStamina { get; }
    bool CanPerformAttack();
    void SpendStaminaForAttack();
    void Tick();
}
