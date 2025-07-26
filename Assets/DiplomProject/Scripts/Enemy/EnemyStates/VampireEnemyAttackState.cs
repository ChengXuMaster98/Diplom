using UnityEngine;
using UnityEngine.AI;

public class VampireEnemyAttackState : IEnemyState
{
    private readonly IEnemyAnimator _animator;
    private readonly NavMeshAgent _agent;
    private readonly IPlayerDamageable _playerDamageable;
    private readonly EnemyStats _stats;
    private readonly IPlayerDetector _detector;
    private readonly VampireEnemyStateMachine _stateMachine;
    private readonly IEnemyStateFactory _stateFactory;

    private float _attackCooldown;

    public VampireEnemyAttackState(
        IPlayerDamageable playerDamageable,
        IEnemyAnimator animator,
        IPlayerDetector detector,
        EnemyStats stats,
        VampireEnemyStateMachine stateMachine,
        NavMeshAgent agent,
        IEnemyStateFactory stateFactory)
    {
        _playerDamageable = playerDamageable;
        _animator = animator;
        _detector = detector;
        _stats = stats;
        _stateMachine = stateMachine;
        _agent = agent;
        _stateFactory = stateFactory;

        _detector.PlayerLost += OnPlayerLost;
    }

    public void Enter()
    {
        _attackCooldown = 0f;
        _agent.isStopped = true;
        _agent.ResetPath();

        _animator.SetAttackHitCallback(PerformAttack);

        Debug.Log("[ATTACK STATE] Entered");
    }

    private void PerformAttack()
{
    if (_detector.Player != null && !_playerDamageable.IsDead)
    {
            Debug.Log($"[VampireEnemy] Наносит {_stats.Damage} урона игроку.");
            _playerDamageable.TakeDamage(_stats.Damage);
    }
}

    public void Tick()
    {
        Transform player = _detector.Player;
        if (player == null)
            return;

        float distance = Vector3.Distance(player.position, _animator.Transform.position);
        float buffer = 0.5f;

        if (distance > _stats.AttackRange + buffer)
        {
            _stateMachine.SetState(_stateFactory.CreateChaseState());
            return;
        }

        _animator.LookAt(player.position);

        _attackCooldown -= Time.deltaTime;

        if (_attackCooldown <= 0f && !_animator.IsPlayingAttackAnimation())
        {
            Debug.Log("[ATTACK] Performing attack!");
            //_playerDamageable.TakeDamage(_stats.Damage);
            _animator.PlayAttack();
            _attackCooldown = _stats.AttackCooldown;
        }
    }

    public void Exit()
    {
        _agent.isStopped = false;
        _agent.updatePosition = true;
        _agent.updateRotation = true;

        _detector.PlayerLost -= OnPlayerLost;

        Debug.Log("[ATTACK STATE] Exited");
    }

    private void OnPlayerLost()
    {
        Debug.Log("[ATTACK] Player lost → switching to Idle");
        _stateMachine.SetState(_stateFactory.CreateIdleState());
    }
}