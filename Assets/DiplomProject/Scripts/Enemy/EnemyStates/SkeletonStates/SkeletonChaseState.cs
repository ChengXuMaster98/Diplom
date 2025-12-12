using UnityEngine;
using UnityEngine.AI;

    public class SkeletonChaseState : IEnemyState
{
    private readonly NavMeshAgent _agent;
    private readonly ISkeletonAnimator _animator;
    private readonly IPlayerDetector _detector;
    private EnemyStats _enemyStats;
    private readonly ISkeletonStateMachine _stateMachine;
    private readonly ISkeletonStateFactory _stateFactory;

    private float _repathInterval = 0.2f;
    private float _nextRepathTime;

    private readonly EnemySoundController _sound;
    private float _vocalTimer;



    public SkeletonChaseState(ISkeletonAnimator animator, NavMeshAgent agent, IPlayerDetector detector, EnemyStats enemyStats, ISkeletonStateMachine stateMachine,
        ISkeletonStateFactory stateFactory, EnemySoundController sound)
    {
        _stateMachine = stateMachine;
        _stateFactory = stateFactory;
        _detector = detector;
        _animator = animator;
        _agent = agent;
        _enemyStats = enemyStats;

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

        _nextRepathTime = 0f;
        _vocalTimer = _sound.GetRandomAggroInterval();
    }

    public void Tick()
    {
        Transform player = _detector.Player;
        if (player == null)
            return;



        float distance = Vector3.Distance(_agent.transform.position, player.position);


        if (distance <= _enemyStats.AttackRange)
        {
            Debug.Log("[CHASE TICK] Switching to Attack state");
            _agent.isStopped = true;
            var attackState = _stateFactory.CreateAttackState() as SkeletonAttackState;
            _stateMachine.SetState(attackState);
            return;
        }
        if (Time.time >= _nextRepathTime)
        {
            _nextRepathTime = Time.time + _repathInterval;
            _agent.isStopped = false;
            _agent.SetDestination(player.position);
        }

        //Фразы при погоне
        _vocalTimer -= Time.deltaTime;
        if (_vocalTimer <= 0f)
        {
            _sound.PlayAggroVocal();
            _vocalTimer = _sound.GetRandomAggroInterval();
        }
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
