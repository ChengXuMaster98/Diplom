using UnityEngine;
using UnityEngine.AI;

public class VampireEnemyChaseState : IEnemyState
{
    private readonly NavMeshAgent _agent;
    private readonly IEnemyAnimator _animator;
    private readonly IPlayerDetector _detector;
    private EnemyStats _enemyStats;
    private readonly VampireEnemyStateMachine _stateMachine;
    private readonly IEnemyStateFactory _stateFactory;

    public VampireEnemyChaseState(IEnemyAnimator animator, NavMeshAgent agent, IPlayerDetector detector, EnemyStats enemyStats, VampireEnemyStateMachine stateMachine,
        IEnemyStateFactory stateFactory)
    {
        _agent = agent;
        _animator = animator;
        _detector = detector;
        _enemyStats = enemyStats;
        _stateMachine = stateMachine;
        _stateFactory = stateFactory;

        //_agent.stoppingDistance = _enemyStats.AttackRange;
        _detector.PlayerLost += OnPlayerLost;
    }

    public void Enter()
    {
        Debug.Log($"[CHASE ENTER] Agent enabled: {_agent.enabled}, isStopped: {_agent.isStopped}, hasPath: {_agent.hasPath}");
        _animator.PlayChase();

        _agent.isStopped = false;
        _agent.updatePosition = true;
        _agent.updateRotation = true;
        _agent.stoppingDistance = _enemyStats.AttackRange;
    }

    public void Tick()
    {
        Transform player = _detector.Player;
        if (player == null)
            return;

        
        

        float distance = Vector3.Distance(_agent.transform.position, player.position);
        Debug.Log($"[CHASE TICK] Distance to player: {distance}, AttackRange: {_enemyStats.AttackRange}, Agent isStopped: {_agent.isStopped}");

        if (distance <= _enemyStats.AttackRange)
        {
            Debug.Log("[CHASE TICK] Switching to Attack state");
            _agent.isStopped = true;
            var attackState = _stateFactory.CreateAttackState() as VampireEnemyAttackState;
            _stateMachine.SetState(attackState);
            return;
        }

        _agent.isStopped = false;
        _agent.SetDestination(player.position);
    }

    public void Exit()
    {
        _detector.PlayerLost -= OnPlayerLost;
    }

    private void OnPlayerLost()
    {
        Debug.Log("[ATTACK STATE] Player lost, switching to Idle");
        _stateMachine.SetState(_stateFactory.CreateIdleState());
    }
}