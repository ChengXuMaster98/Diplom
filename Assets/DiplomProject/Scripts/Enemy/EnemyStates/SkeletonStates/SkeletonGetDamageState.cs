using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGetDamageState : IEnemyState
{
    private readonly Animator _animator;
    private readonly ISkeletonStateMachine _stateMachine;
    private readonly ISkeletonStateFactory _factory;

    private float _timer;


    public SkeletonGetDamageState(Animator animator, ISkeletonStateMachine stateMachine, ISkeletonStateFactory factory)
    {
        _animator = animator;
        _stateMachine = stateMachine;
        _factory = factory;

    }

    public void Enter()
    {
        _timer = 0.3f;   // длительность анимации удара
        _animator.Play("Impact");
    }

    public void Exit()
    {

    }

    public void Tick()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            // ¬озврат в Idle или Chase
            _stateMachine.SetState(_factory.CreateIdleState());
        }
    }
}

