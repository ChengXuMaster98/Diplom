using UnityEngine;
using UnityEngine.AI;

public class BossPatrolState : IEnemyState
{
    private readonly IBossAnimator _animator;
    private readonly IBossStateMachine _stateMachine;
    private readonly IBossStateFactory _factory;
    private readonly IPlayerDetector _detector;
    private readonly NavMeshAgent _agent;

    private Vector3 _origin;
    private bool _originSet;

    private readonly float _patrolRadius = 6f;
    private readonly float _waitAtPoint = 1.5f;

    private float _waitTimer;

    private readonly EnemySoundController _sound;
    private float _vocalTimer;


    public BossPatrolState(
        IBossAnimator animator,
        IBossStateMachine stateMachine,
        IBossStateFactory factory,
        IPlayerDetector detector,
        NavMeshAgent agent,
        EnemySoundController sound)
    {
        _animator = animator;
        _stateMachine = stateMachine;
        _factory = factory;
        _detector = detector;
        _agent = agent;
        _sound = sound;
    }

    public void Enter()
    {
        Debug.Log("[Skeleton] Enter Patrol");

        if (!_originSet)
        {
            _origin = _agent.transform.position;   // стартовая точка патруля
            _originSet = true;
        }


        _agent.isStopped = false;
        _agent.updatePosition = true;
        _agent.updateRotation = true;
        _agent.stoppingDistance = 0f;

        _vocalTimer = _sound.GetRandomIdleInterval();

        _waitTimer = 0f;
        SetNextDestination();
        _animator.PlayPatrol();

        _detector.PlayerDetected += OnPlayerDetected;
    }

    public void Tick()
    {

        // Если игрок появился — сразу в Chase
        if (_detector.Player != null)
            return; // OnPlayerDetected сам переключит


        if (_agent.pathPending)
            return;

        if (_agent.remainingDistance <= 0.2f)
        {
            // Если только что дошёл — включаем Idle
            if (_waitTimer == 0f)
                _animator.PlayIdle();

            _waitTimer += Time.deltaTime;

            if (_waitTimer >= _waitAtPoint)
            {
                _waitTimer = 0f;
                SetNextDestination();
                _animator.PlayPatrol(); // ← снова запускаем patrol
            }

            _vocalTimer -= Time.deltaTime;
            if (_vocalTimer <= 0f)
            {
                _sound.PlayIdle();
                _vocalTimer = _sound.GetRandomIdleInterval();
            }

            return;
        }

        _animator.PlayPatrol();
    }

    public void Exit()
    {

        _detector.PlayerDetected -= OnPlayerDetected;
    }

    private void SetNextDestination()
    {
        Vector2 randomCircle = Random.insideUnitCircle * _patrolRadius;
        Vector3 rawTarget = _origin + new Vector3(randomCircle.x, 0f, randomCircle.y);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(rawTarget, out hit, _patrolRadius, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }
        else
        {
            // fallback — просто идёт в исходную точку
            _agent.SetDestination(_origin);
        }
    }

    private void OnPlayerDetected(Transform player)
    {
        Debug.Log("[Skeleton] Player detected from Patrol → Chase");
        _stateMachine.SetState(_factory.CreateChaseState());
    }
}
