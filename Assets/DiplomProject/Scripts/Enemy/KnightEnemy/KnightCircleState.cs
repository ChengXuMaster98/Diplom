using UnityEngine;
using UnityEngine.AI;

public class KnightCircleState : IEnemyState
{
    private readonly IKnightAnimator _animator;
    private readonly IKnightStateMachine _machine;
    private readonly IKnightStateFactory _factory;
    private readonly IPlayerDetector _detector;
    private readonly NavMeshAgent _agent;
    private readonly EnemyStats _stats;

    private float _timeInState;
    private float _nextAttackTime;
    private float _side = -1f;              // -1f или 1f

    private const float DesiredDistance = 2.5f;

    public KnightCircleState(
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
        _agent.stoppingDistance = 0f;

        _timeInState = 0f;
        _nextAttackTime = Random.Range(1.0f, 2.0f);

        // рандом направления кружения
        _side = Random.value > 0.5f ? 1f : -1f;

        _animator.SetRootMotion(false);
        _animator.PlayCircle(_side);
    }

    public void Tick()
    {
        var player = _detector.Player;
        if (player == null)
        {
            _machine.SetState(_factory.CreateIdleState());
            return;
        }

        _timeInState += Time.deltaTime;

        // Вектор на игрока
        Vector3 toPlayer = player.position - _agent.transform.position;
        toPlayer.y = 0f;

        float dist = toPlayer.magnitude;

        // если вдруг отошли слишком далеко - опять приближаемся
        if (dist > DesiredDistance + 1.0f)
        {
            _machine.SetState(_factory.CreateChaseState());
            return;
        }

        Vector3 dirToPlayer = toPlayer.sqrMagnitude > 0.0001f
            ? toPlayer.normalized
            : Vector3.forward;

        // вектор вдоль окружности
        Vector3 tangent = Vector3.Cross(Vector3.up, dirToPlayer) * _side;

        // целевая позиция на дуге
        Vector3 circleTarget =
            player.position
          - dirToPlayer * DesiredDistance
          + tangent * 1.5f;

        _agent.SetDestination(circleTarget);
        _animator.LookAt(player.position);
        _animator.PlayCircle(_side);   // поддерживаем blend

        // готов ли к атаке
        if (_timeInState >= _nextAttackTime)
        {
            _machine.SetState(_factory.CreateAttackState());
        }
    }

    public void Exit()
    {
        _agent.isStopped = true;
    }
}