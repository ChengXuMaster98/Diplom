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
    private readonly EnemySoundController _sound;
    private float _vocalTimer;

    public VampireEnemyChaseState(IEnemyAnimator animator, NavMeshAgent agent, IPlayerDetector detector, EnemyStats enemyStats, VampireEnemyStateMachine stateMachine,
        IEnemyStateFactory stateFactory, EnemySoundController sound)
    {
        _agent = agent;
        _animator = animator;
        _detector = detector;
        _enemyStats = enemyStats;
        _stateMachine = stateMachine;
        _stateFactory = stateFactory;

        _detector.PlayerLost += OnPlayerLost;
        _sound = sound;
    }

    public void Enter()
    {

        _animator.PlayChase();

        _agent.isStopped = false;
        _agent.updatePosition = true;
        _agent.updateRotation = true;
        _agent.stoppingDistance = _enemyStats.AttackRange;

        _vocalTimer = _sound.GetRandomAggroInterval();
    }

    public void Tick()
    {
        Transform player = _detector.Player;
        if (player == null)
            return;

        //Реплики при агре
        _vocalTimer -= Time.deltaTime;
        if (_vocalTimer <= 0f)
        {
            _sound.PlayAggroVocal();
            _vocalTimer = _sound.GetRandomAggroInterval();
        }



        float distance = Vector3.Distance(_agent.transform.position, player.position);


        if (distance <= _enemyStats.AttackRange)
        {
            Debug.Log("[CHASE TICK] Switching to Attack state");
            _agent.isStopped = true;
            var attackState = _stateFactory.CreateAttackState() as VampireEnemyAttackState;
            _stateMachine.SetState(attackState);
            return;
        }


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