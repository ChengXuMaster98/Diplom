using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface IStateFactory
{
    IEnemyState CreateIdleState();
    IEnemyState CreateChaseState();
    IEnemyState CreateAttackState();
    IEnemyState CreateDieState();

    IEnemyState CreateGetDamageState();
}
