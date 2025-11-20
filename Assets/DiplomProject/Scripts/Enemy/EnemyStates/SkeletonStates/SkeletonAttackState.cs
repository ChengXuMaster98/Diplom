using UnityEngine;
using UnityEngine.AI;

    public class SkeletonAttackState : IEnemyState
{
    private readonly ISkeletonAnimator _animator;
    private readonly EnemyStats _stats;
    private readonly NavMeshAgent _agent;
    private readonly IPlayerDamageable _playerDamageable;
    private readonly IPlayerDetector _detector;
    
    private readonly ISkeletonStateMachine _stateMachine;
    private readonly ISkeletonStateFactory _stateFactory;

    private float _attackCooldown;

    //private const float AttackToChaseThreshold = 1.2f;

    public SkeletonAttackState(
        IPlayerDamageable playerDamageable,
        ISkeletonAnimator animator,
        IPlayerDetector detector,
        EnemyStats stats,
        ISkeletonStateMachine stateMachine,
        NavMeshAgent agent,
        ISkeletonStateFactory stateFactory)
    {
        _playerDamageable = playerDamageable;
        Debug.Log($"[AttackState] PlayerDamageable is null? {_playerDamageable == null}");
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

        _agent.isStopped = true;
        _agent.ResetPath();



        _attackCooldown = 0f;

        _animator.SetAttackHitCallback(PerformAttack);

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

        Debug.Log("[ATTACK] About to call TakeDamage on _playerDamageable");
        _playerDamageable.TakeDamage(_stats.Damage);
        Debug.Log("[ATTACK] After TakeDamage call");
    }



    public void Tick()
    {
        Debug.Log($"[Attack Tick] Player: {_detector.Player}, Damageable: {_playerDamageable != null}");
        Transform player = _detector.Player;
        if (player == null)
            return;

        float distance = Vector3.Distance(player.position, _animator.Transform.position);
        //float buffer = 0.5f;

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
            _agent.isStopped = true;
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
