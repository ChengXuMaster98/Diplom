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
    
    private readonly EnemySoundController _sound;
    private float _vocalTimer;


    private float _attackCooldown;

    public VampireEnemyAttackState(
        IPlayerDamageable playerDamageable,
        IEnemyAnimator animator,
        IPlayerDetector detector,
        EnemyStats stats,
        VampireEnemyStateMachine stateMachine,
        NavMeshAgent agent,
        IEnemyStateFactory stateFactory,
        EnemySoundController sound)
    {
        _playerDamageable = playerDamageable;
        Debug.Log($"[AttackState] PlayerDamageable is null? {_playerDamageable == null}");
        _animator = animator;
        _detector = detector;
        _stats = stats;
        _stateMachine = stateMachine;
        _agent = agent;
        _stateFactory = stateFactory;
        _sound = sound;

        _detector.PlayerLost += OnPlayerLost;
    }

    public void Enter()
    {

        _attackCooldown = 0f;
        _agent.isStopped = true;
        _agent.ResetPath();

        _animator.SetAttackHitCallback(PerformAttack);
        _animator.PlayAttack();

        _vocalTimer = _sound.GetRandomAttackInterval();

        Debug.Log("[ATTACK STATE] Entered");
    }

    private void PerformAttack()
    {
        if (_detector.Player == null || _playerDamageable == null)
            return;

        float distance = Vector3.Distance(_detector.Player.position, _animator.Transform.position);
        if (distance > _stats.AttackRange)
        {
            Debug.Log("[Attack] Player escaped before hit!");
            return;
        }

        _playerDamageable.TakeDamage(_stats.Damage);
        Debug.Log("[Attack] Damage applied");
    }

    public void Tick()
    {
        Debug.Log($"[Attack Tick] Player: {_detector.Player}, Damageable: {_playerDamageable != null}");
        Transform player = _detector.Player;
        if (player == null)
            return;

        float distance = Vector3.Distance(player.position, _animator.Transform.position);
        //float buffer = 0.1f;

        if (distance > _stats.AttackRange)
        {
            Debug.Log("[Attack] Too far, switching to Chase");
            _stateMachine.SetState(_stateFactory.CreateChaseState());
            return;
        }
        else
        {
            Debug.Log("[Attack] Player in AttackRange");
        }


        _animator.LookAt(player.position);

        _attackCooldown -= Time.deltaTime;

        if (_attackCooldown <= 0f && !_animator.IsPlayingAttackAnimation())
        {
            Debug.Log("[ATTACK] Performing attack!");

            _animator.PlayAttack();
            _attackCooldown = _stats.AttackCooldown;
        }

        _vocalTimer -= Time.deltaTime;
        if (_vocalTimer <= 0f)
        {
            _sound.PlayAttack();
            _vocalTimer = _sound.GetRandomAttackInterval();
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