using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class SkeletonChaseState : IEnemyState
{
    private readonly ISkeletonStateMachine _machine;
    private readonly ISkeletonStateFactory _factory;
    private readonly Transform _transform;
    private readonly IPlayerDetector _detector;
    private readonly Animator _animator;
    private readonly EnemyStats _stats;

    private Transform _target;

    public SkeletonChaseState(ISkeletonStateMachine machine, ISkeletonStateFactory factory, Transform transform, IPlayerDetector detector, Animator animator, EnemyStats stats)
    {
        _machine = machine;
        _factory = factory;
        _transform = transform;
        _detector = detector;
        _animator = animator;
        _stats = stats;
    }

    public void Enter()
    {
        _animator.Play("Run");
        _detector.PlayerDetected += OnPlayerDetected;
        _detector.PlayerLost += OnPlayerLost;
    }

    public void Tick()
    {
        if (_target == null) return;

        Vector3 dir = (_target.position - _transform.position).normalized;
        _transform.position += dir * _stats.MoveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(_transform.position, _target.position);
        if (distance <= _stats.AttackRange)
            _machine.SetState(_factory.CreateAttackState());
    }

    public void Exit()
    {
        _detector.PlayerDetected -= OnPlayerDetected;
        _detector.PlayerLost -= OnPlayerLost;
    }

    private void OnPlayerDetected(Transform player) => _target = player;
    private void OnPlayerLost() => _target = null;
}
