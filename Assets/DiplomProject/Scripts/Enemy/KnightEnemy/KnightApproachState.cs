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

    private const float DesiredDistance = 2.5f;

    public KnightApproachState(
        IKnightAnimator animator,
        IKnightStateMachine machine,
        IKnightStateFactory factory,
        IPlayerDetector detector,
        NavMeshAgent agent,
        EnemyStats stats)
    {
        _animator = animator;
        _machine = machine;
        _factory = factory;
        _detector = detector;
        _agent = agent;
        _stats = stats;
    }

    public void Enter()
    {
        _agent.isStopped = false;
        _agent.updatePosition = true;
        _agent.updateRotation = true;
        _agent.stoppingDistance = DesiredDistance;

        _animator.SetRootMotion(false);
        _animator.PlayMove();
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

        float dist = Vector3.Distance(_agent.transform.position, player.position);
        if (dist <= DesiredDistance + 0.2f)
        {
            _machine.SetState(_factory.CreateCircleState());
        }
    }

    public void Exit()
    {
        _agent.isStopped = true;
    }
}