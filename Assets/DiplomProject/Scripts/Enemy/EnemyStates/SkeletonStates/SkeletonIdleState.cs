using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class SkeletonIdleState : IEnemyState
{
    private readonly ISkeletonStateMachine _machine;
    private readonly ISkeletonStateFactory _factory;
    private readonly SkeletonStateMachine _stateMachine;
    private readonly Transform _transform;
    private readonly Animator _animator;
    private readonly IPlayerDetector _detector;

    private Transform _player;

    public SkeletonIdleState(ISkeletonStateMachine machine, IPlayerDetector detector, ISkeletonStateFactory factory, Transform transform, Animator animator)
    {
        _machine = machine;
        _factory = factory;
        _transform = transform;
        _animator = animator;
        _detector = detector;

        _detector.PlayerDetected += OnPlayerDetected;
    }

    public void Enter()
    {
        _animator.Play("Idle");
    }

    public void Tick()
    {
        if (_player != null)
        {
            var chaseState = _factory.CreateChaseState() as SkeletonChaseState;
            _stateMachine.SetState(chaseState);
        }
    }

    public void Exit()
    {
        _detector.PlayerDetected -= OnPlayerDetected;
    }

    private void OnPlayerDetected(Transform player)
    {
        _player = player;
    }
}
