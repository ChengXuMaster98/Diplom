using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class SkeletonAttackState : IEnemyState
{
    private readonly ISkeletonStateMachine _machine;
    private readonly ISkeletonStateFactory _factory;
    private readonly Transform _transform;
    private readonly Animator _animator;
    private readonly EnemyStats _stats;

    private float _attackCooldown;

    public SkeletonAttackState(ISkeletonStateMachine machine, ISkeletonStateFactory factory, Transform transform, Animator animator, EnemyStats stats)
    {
        _machine = machine;
        _factory = factory;
        _transform = transform;
        _animator = animator;
        _stats = stats;
    }

    public void Enter()
    {
        _animator.Play("Attack");
        _attackCooldown = _stats.AttackCooldown;
    }

    public void Tick()
    {
        _attackCooldown -= Time.deltaTime;
        if (_attackCooldown <= 0f)
        {
            _machine.SetState(_factory.CreateChaseState());
        }
    }

    public void Exit() { }
}
