using UnityEngine;

public class SkeletonEnemyAI : EnemyAIBase<SkeletonStateMachine, ISkeletonStateFactory>
{
    private IEnemyState _idleState;
    private IEnemyState _chaseState;
    private IEnemyState _attackState;
    private IEnemyState _flyState;
    private IEnemyState _dieState;

    public override void Initialize()
    {
        _idleState = _stateFactory.CreateIdleState();
        _chaseState = _stateFactory.CreateChaseState();
        _attackState = _stateFactory.CreateAttackState();
        _flyState = _stateFactory.CreateFlyState();
        _dieState = _stateFactory.CreateDieState();

        _stateMachine.Initialize(_idleState);
    }

    protected override void OnPlayerDetected(Transform player)
    {
        _stateMachine.SetState(_chaseState);
    }

    protected override void OnPlayerLost()
    {
        _stateMachine.SetState(_idleState);
    }
}
