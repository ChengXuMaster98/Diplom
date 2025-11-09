using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class SkeletonFlyState : IEnemyState
{
    private readonly ISkeletonStateMachine _machine;
    private readonly ISkeletonStateFactory _factory;
    private readonly Transform _transform;
    private readonly Animator _animator;
    private float _floatHeight = 2f;
    private float _floatSpeed = 2f;
    private float _startY;

    public SkeletonFlyState(ISkeletonStateMachine machine, ISkeletonStateFactory factory, Transform transform, Animator animator)
    {
        _machine = machine;
        _factory = factory;
        _transform = transform;
        _animator = animator;
    }

    public void Enter()
    {
        _startY = _transform.position.y;
        _animator.Play("Float");
    }

    public void Tick()
    {
        _transform.position = new Vector3(
            _transform.position.x,
            _startY + Mathf.Sin(Time.time * _floatSpeed) * _floatHeight,
            _transform.position.z
        );
    }

    public void Exit() { }
}
