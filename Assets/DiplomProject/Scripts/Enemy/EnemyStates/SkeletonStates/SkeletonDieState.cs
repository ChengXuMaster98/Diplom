using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class SkeletonDieState : IEnemyState
{
    private readonly ISkeletonStateMachine _machine;
    private readonly ISkeletonStateFactory _factory;
    private readonly Transform _transform;
    private readonly Animator _animator;

    public SkeletonDieState(ISkeletonStateMachine machine, ISkeletonStateFactory factory, Transform transform, Animator animator)
    {
        _machine = machine;
        _factory = factory;
        _transform = transform;
        _animator = animator;
    }

    public void Enter()
    {
        _animator.Play("Die");
    }

    public void Tick() { }

    public void Exit() { }
}
