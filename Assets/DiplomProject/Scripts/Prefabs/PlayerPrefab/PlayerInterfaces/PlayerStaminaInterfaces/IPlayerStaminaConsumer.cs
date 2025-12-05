using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface IPlayerStaminaConsumer
{
    bool CanAttack();
    void ConsumeStaminaForAttack();

    bool CanBlock();
    void ConsumeStaminaForBlock();

    bool CanDash();

    void ConsumeStaminaForDash();
}