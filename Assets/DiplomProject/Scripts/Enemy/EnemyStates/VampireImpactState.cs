using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireImpactState : IEnemyState
{
    private readonly IEnemyAnimator _animator;


    public VampireImpactState(IEnemyAnimator animator)
    {
        _animator = animator;
    }

    public void Enter()
    {
        _animator.PlayImpact();
    }
    
    public void Exit()
    {

    }

    public void Tick()
    {

    }
}
