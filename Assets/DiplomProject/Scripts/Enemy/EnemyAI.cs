using UnityEngine;
using Zenject;

public class EnemyAI : MonoBehaviour, IInitializable, ITickable
{
    private EnemyStateMachine _stateMachine;

    private IEnemyState _idleState;
    private EnemyChaseState _chaseState;
    private EnemyAttackState _attackState;
    private IEnemyState _dieState;

    private DetectionArea _detectionArea;
    private IPlayerDamageable _damageable;

    private Transform _target;

    [Inject]
    public void Construct(
        EnemyStateMachine stateMachine,
        EnemyIdleState idle,
        EnemyChaseState chase,
        EnemyAttackState attack,
        EnemyDieState die,
        DetectionArea detectionArea)
    {
        _stateMachine = stateMachine;

        _dieState = die;
        _idleState = idle;
        _chaseState = chase;
        _attackState = attack;
        _detectionArea = detectionArea;
    }

    public void Initialize()
    {
        _detectionArea.PlayerEntered += OnPlayerEntered;
        _detectionArea.PlayerExited += OnPlayerExited;
        _stateMachine.Initialize(_idleState);
    }

    public void Tick()
    {
        _stateMachine.Tick();
    }

    public void Die()
    {
        _detectionArea.PlayerEntered -= OnPlayerEntered;
        _detectionArea.PlayerExited -= OnPlayerExited;
        _stateMachine.SetState(_dieState);
    }

    private void OnPlayerEntered(Transform player)
    {
        Debug.Log($"[EnemyAI] Игрок вошёл в DetectionArea: {player.name}");

        _damageable = player.GetComponent<IPlayerDamageable>();
        _target = player;
        _chaseState.SetTarget(player);
        _attackState.SetTarget(player);

        Debug.Log("[EnemyAI] Переход в ChaseState");
        _stateMachine.SetState(_chaseState);
    }

    private void OnPlayerExited()
    {
        _target = null;
        _stateMachine.SetState(_idleState);
    }
}