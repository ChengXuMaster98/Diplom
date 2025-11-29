using UnityEngine;
using UnityEngine.AI;

public class KnightAttackState : IEnemyState
{
    private readonly IKnightAnimator _animator;
    private readonly IKnightStateMachine _machine;
    private readonly IKnightStateFactory _factory;
    private readonly IPlayerDetector _detector;
    private readonly IPlayerDamageable _playerDamageable;
    private readonly EnemyStats _stats;
    private readonly NavMeshAgent _agent;

    private float _attackCooldown;
    private int _attacksDone;
    private int _attacksInBurst;

    public KnightAttackState(
        IKnightAnimator animator,
        IKnightStateMachine machine,
        IKnightStateFactory factory,
        IPlayerDetector detector,
        IPlayerDamageable playerDamageable,
        EnemyStats stats,
        NavMeshAgent agent)
    {
        _animator = animator;
        _machine = machine;
        _factory = factory;
        _detector = detector;
        _playerDamageable = playerDamageable;
        _stats = stats;
        _agent = agent;
    }

    public void Enter()
    {
        _agent.isStopped = true;
        _attackCooldown = 0f;
        _attacksDone = 0;
        _attacksInBurst = Random.Range(1, 3); // 1–2 удара максимум

        _animator.SetAttackHitCallback(PerformAttack);
    }

    private void PerformAttack()
    {
        if (_detector.Player == null) return;
        if (_playerDamageable == null) return;

        float dist = Vector3.Distance(
            _detector.Player.position,
            _animator.Transform.position);

        if (dist > _stats.AttackRange + 0.3f)
            return;

        _playerDamageable.TakeDamage(_stats.Damage);
    }

    public void Tick()
    {
        var player = _detector.Player;
        if (player == null)
        {
            _machine.SetState(_factory.CreateIdleState());
            return;
        }

        _animator.LookAt(player.position);

        float dist = Vector3.Distance(player.position, _animator.Transform.position);

        if (dist > _stats.AttackRange + 0.5f)
        {
            _machine.SetState(_factory.CreateChaseState());
            return;
        }

        // ждём окно для атаки
        _attackCooldown -= Time.deltaTime;

        if (_attackCooldown <= 0f && !_animator.IsPlayingAttack())
        {
            _animator.PlayAttack();
            _attacksDone++;
            _attackCooldown = Random.Range(0.8f, 1.6f); // пауза между ударами

            if (_attacksDone >= _attacksInBurst)
            {
                // после серии уходим назад
                _machine.SetState(_factory.CreateRetreatState());
            }
        }
    }

    public void Exit()
    {
        _agent.isStopped = false;
    }
}