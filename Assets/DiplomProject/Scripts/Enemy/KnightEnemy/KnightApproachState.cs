using UnityEngine;
using UnityEngine.AI;

public class KnightApproachState : IEnemyState
{
    private readonly IKnightAnimator _animator;
    private readonly IKnightStateMachine _machine;
    private readonly IKnightStateFactory _factory;
    private readonly IPlayerDetector _detector;
    private readonly NavMeshAgent _agent;
    private readonly EnemyStats _stats;

    private readonly EnemySoundController _sound;
    private float _vocalTimer;


    public KnightApproachState(
        IKnightAnimator animator,
        IKnightStateMachine machine,
        IKnightStateFactory factory,
        IPlayerDetector detector,
        NavMeshAgent agent,
        EnemyStats stats,
        EnemySoundController sound)
    {
        _animator = animator;
        _machine = machine;
        _factory = factory;
        _detector = detector;
        _agent = agent;
        _stats = stats;
        _sound = sound;
    }

    public void Enter()
    {
        _agent.isStopped = false;
        _agent.updatePosition = true;
        _agent.updateRotation = true;
        _agent.stoppingDistance = _stats.AttackRange;

        _animator.SetRootMotion(false);
        _animator.PlayMove();

        _vocalTimer = _sound.GetRandomAggroInterval();
    }

    public void Tick()
    {
        var player = _detector.Player;
        if (player == null)
        {
            _machine.SetState(_factory.CreateIdleState());
            return;
        }

        _agent.SetDestination(player.position);
        _animator.LookAt(player.position);

        _vocalTimer -= Time.deltaTime;
        if (_vocalTimer <= 0f)
        {
            _sound.PlayAggroVocal();
            _vocalTimer = _sound.GetRandomAggroInterval();
        }


        float dist = Vector3.Distance(_agent.transform.position, player.position);
        if (dist <= _stats.AttackRange + 0.2f)
        {
            if (_machine.AttackIntent)
            {

                _machine.SetState(_factory.CreateAttackState());
            }
            else
            {

                _machine.SetState(_factory.CreateCircleState());
            }
        }

    }

    public void Exit()
    {
        _agent.isStopped = true;
    }
}