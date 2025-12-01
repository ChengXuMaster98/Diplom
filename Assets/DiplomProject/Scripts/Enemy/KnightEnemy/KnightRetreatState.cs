using UnityEngine;
using UnityEngine.AI;

public class KnightRetreatState : IEnemyState
{
    private readonly IKnightAnimator _animator;
    private readonly IKnightStateMachine _machine;
    private readonly IKnightStateFactory _factory;
    private readonly IPlayerDetector _detector;
    private readonly NavMeshAgent _agent;

    private float _timer;
    private const float RetreatDuration = 1.0f;
    private const float RetreatDistance = 2.0f;

    private Vector3 _targetPos;

    public KnightRetreatState(
        IKnightAnimator animator,
        IKnightStateMachine machine,
        IKnightStateFactory factory,
        IPlayerDetector detector,
        NavMeshAgent agent)
    {
        _animator = animator;
        _machine = machine;
        _factory = factory;
        _detector = detector;
        _agent = agent;
    }

    public void Enter()
    {
        _timer = 0f;

        var player = _detector.Player;
        Vector3 current = _agent.transform.position;

        Vector3 dirFromPlayer = Vector3.zero;

        if (player != null)
        {
            dirFromPlayer = current - player.position;
            dirFromPlayer.y = 0f;

            if (dirFromPlayer.sqrMagnitude > 0.0001f)
                dirFromPlayer = dirFromPlayer.normalized;
            else
                dirFromPlayer = _agent.transform.forward * -1f;
        }
        else
        {
            dirFromPlayer = _agent.transform.forward * -1f;
        }

        _targetPos = current + dirFromPlayer * RetreatDistance;

        _agent.isStopped = false;
        _agent.updatePosition = true;
        _agent.updateRotation = true;
        _agent.SetDestination(_targetPos);

        _animator.SetRootMotion(false);
        _animator.PlayRetreat();
    }

    public void Tick()
    {
        _timer += Time.deltaTime;

        if (_timer >= RetreatDuration ||
           (!_agent.pathPending && _agent.remainingDistance <= 0.2f))
        {
            _machine.SetState(_factory.CreateCircleState());
        }
    }

    public void Exit()
    {
        _agent.isStopped = true;
    }
}