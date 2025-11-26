using UnityEngine;

public class BossEnemyAI : EnemyAIBase<BossStateMachine, IBossStateFactory>
{
    private IEnemyState _idleState;
    private IEnemyState _chaseState;
    private IEnemyState _attackState;
    private IEnemyState _dieState;
    private IEnemyState _getDamageState;
    private IEnemyState _patrolState;

    public override void Initialize()
    {
        _idleState = _stateFactory.CreateIdleState();
        _chaseState = _stateFactory.CreateChaseState();
        _attackState = _stateFactory.CreateAttackState();
        _getDamageState = _stateFactory.CreateGetDamageState();
        _dieState = _stateFactory.CreateDieState();
        _patrolState = _stateFactory.CreatePatrolState();


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
