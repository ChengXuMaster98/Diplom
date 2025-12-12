using UnityEngine;
using UnityEngine.AI;

public class SkinnyOrkChaseState : IEnemyState
{
    private readonly NavMeshAgent _agent;
    private readonly ISkinnyOrkAnimator _animator;
    private readonly IPlayerDetector _detector;
    private EnemyStats _enemyStats;
    private readonly ISkinnyOrkStateMachine _stateMachine;
    private readonly ISkinnyOrkStateFactory _stateFactory;

    private readonly EnemySoundController _sound;
    private float _vocalTimer;


    public SkinnyOrkChaseState(ISkinnyOrkAnimator animator, NavMeshAgent agent, IPlayerDetector detector, EnemyStats enemyStats, ISkinnyOrkStateMachine stateMachine,
        ISkinnyOrkStateFactory stateFactory, EnemySoundController sound)
    {
        _stateMachine = stateMachine;
        _stateFactory = stateFactory;
        _detector = detector;
        _animator = animator;
        _agent = agent;
        _enemyStats = enemyStats;
        _sound = sound;

        _detector.PlayerLost += OnPlayerLost;
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



        float distance = Vector3.Distance(_agent.transform.position, player.position);


        _vocalTimer -= Time.deltaTime;
        if (_vocalTimer <= 0f)
        {
            _sound.PlayAggroVocal();
            _vocalTimer = _sound.GetRandomAggroInterval();
        }


        if (distance <= _enemyStats.AttackRange)
        {
            Debug.Log("[CHASE TICK] Switching to Attack state");
            _agent.isStopped = true;
            var attackState = _stateFactory.CreateAttackState() as SkinnyOrkAttackState;
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
        _stateMachine.SetState(_stateFactory.CreatePatrolState());
    }
}
