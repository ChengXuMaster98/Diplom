using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyAI : MonoBehaviour, IInitializable, ITickable
{
    private EnemyStateMachine _stateMachine;

    private IEnemyState _idleState;
    private IEnemyState _chaseState;
    private IEnemyState _attackState;
    private IEnemyState _dieState;

    private Transform _player;
    private Enemy _enemy;

    private EnemyAnimatorController _animator;
    private NavMeshAgent _agent;

    [Inject]
    public void Construct(
        EnemyStateMachine stateMachine,
        EnemyIdleState idle,
        EnemyChaseState chase,
        EnemyAttackState attack,
        EnemyDieState die,
        Transform player)
    {
        _stateMachine = stateMachine;

        _idleState = idle;
        _chaseState = chase;
        _attackState = attack;
        _dieState = die;

        _player = player;
    }

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _animator = GetComponent<EnemyAnimatorController>();
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Initialize()
    {
        Debug.Log("FSM ��������������� � IdleState");
        _stateMachine.Initialize(_idleState);
    }

    public void Tick()
    {
        if (_enemy.IsDead)
        {
            _stateMachine.SetState(_dieState);
            return;
        }
        float distance = Vector3.Distance(transform.position, _player.position);

        if (distance < 1.5f)
            _stateMachine.SetState(_attackState);
        else if (distance < 10f)
            _stateMachine.SetState(_chaseState);
        else
            _stateMachine.SetState(_idleState);

        _stateMachine.Tick();
    }
}