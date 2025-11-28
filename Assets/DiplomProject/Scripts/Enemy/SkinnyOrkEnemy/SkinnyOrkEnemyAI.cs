using UnityEngine;

public class SkinnyOrkEnemyAI : EnemyAIBase<SkinnyOrkStateMachine, ISkinnyOrkStateFactory>
{
    private IEnemyState _idleState;
    private IEnemyState _chaseState;
    private IEnemyState _attackState;
    private IEnemyState _flyState;
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
        if (!_isAggro)
        {
            _isAggro = true;
            _aggroTracker?.Increment();
        }

        _stateMachine.SetState(_chaseState);
    }

    protected override void OnPlayerLost()
    {
        if (_isAggro)
        {
            _isAggro = false;
            _aggroTracker?.Decrement();
        }

        _stateMachine.SetState(_idleState);
    }
}
