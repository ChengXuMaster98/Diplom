using UnityEngine;

public class KnightEnemyAI : EnemyAIBase<IKnightStateMachine, IKnightStateFactory>
{
    private IEnemyState _idleState;
    private IEnemyState _approachState;  // вместо Chase
    private IEnemyState _attackState;
    private IEnemyState _circleState;
    //private IEnemyState _retreatState;
    private IEnemyState _getDamageState;
    private IEnemyState _dieState;

    public override void Initialize()
    {
        _idleState = _stateFactory.CreateIdleState();
        _approachState = _stateFactory.CreateChaseState();    // используем стандартное имя
        _attackState = _stateFactory.CreateAttackState();
        _circleState = _stateFactory.CreateCircleState();
        //_retreatState = _stateFactory.CreateRetreatState();
        _getDamageState = _stateFactory.CreateGetDamageState();
        _dieState = _stateFactory.CreateDieState();

        _stateMachine.Initialize(_idleState);
    }

    protected override void OnPlayerDetected(Transform player)
    {
        _stateMachine.SetState(_approachState);
    }

    protected override void OnPlayerLost()
    {
        _stateMachine.SetState(_idleState);
    }
}