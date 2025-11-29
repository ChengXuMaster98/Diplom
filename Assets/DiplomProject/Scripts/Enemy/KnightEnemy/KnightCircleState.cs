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
        _agent.stoppingDistance = 0.0f;
        _timeInState = 0f;
        // небольшой рандом, когда можно атаковать
        _nextAttackTime = Random.Range(1.0f, 2.0f);
        _animator.PlayCircle();
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

        // двигаемся по дуге вокруг игрока
        Vector3 toPlayer = (player.position - _agent.transform.position);
        toPlayer.y = 0f;
        float dist = toPlayer.magnitude;

        if (dist > 3.0f)
        {
            // слишком далеко → снова подходим
            _machine.SetState(_factory.CreateChaseState());
            return;
        }

        // направление вдоль "орбиты" (перпендикуляр к вектору на игрока)
        Vector3 tangent = Vector3.Cross(Vector3.up, toPlayer.normalized);
        // можно рандомизировать левое/правое кружение
        float side = 1f; // или -1f
        Vector3 circleTarget = player.position - toPlayer.normalized * 2.2f + tangent * side * 1.5f;

        _agent.SetDestination(circleTarget);
        _animator.LookAt(player.position);

        // готов ли к атаке?
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