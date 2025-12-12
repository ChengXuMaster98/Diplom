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

    private readonly EnemySoundController _sound;
    private float _vocalTimer;

    private float _attackCooldown;
    private int _attacksDone;
    private int _attacksInBurst;

    private Vector3 _lockedTargetPosition;

    public KnightAttackState(
        IKnightAnimator animator,
        IKnightStateMachine machine,
        IKnightStateFactory factory,
        IPlayerDetector detector,
        IPlayerDamageable playerDamageable,
        EnemyStats stats,
        NavMeshAgent agent,
        EnemySoundController sound)
    {
        _animator = animator;
        _machine = machine;
        _factory = factory;
        _detector = detector;
        _playerDamageable = playerDamageable;
        _stats = stats;
        _agent = agent;
        _sound = sound;
    }

    public void Enter()
    {
        _vocalTimer = _sound.GetRandomAttackInterval();

        //стоп и отключение агента. Надо попробовать наоборот не отключать может?
        _agent.isStopped = true;
        _agent.updatePosition = false;
        _agent.updateRotation = false;
        _agent.ResetPath();
        _agent.velocity = Vector3.zero;

        _animator.SetRootMotion(true);

        // фиксация позиции цели на момент начала атаки
        var player = _detector.Player;
        _lockedTargetPosition = player != null
            ? player.position
            : _animator.Transform.position + _animator.Transform.forward;

        _attackCooldown = 0f;
        _attacksDone = 0;
        _attacksInBurst = Random.Range(1, 3);   // от 1 до 2 ударов

        _animator.SetAttackHitCallback(PerformAttack);
    }

    private void PerformAttack()
    {
        if (_playerDamageable == null)
            return;

        float dist = Vector3.Distance(_lockedTargetPosition, _animator.Transform.position);
        if (dist > _stats.AttackRange + 0.3f)
        {
            // промах
            return;
        }

        _playerDamageable.TakeDamage(_stats.Damage);
    }

    public void Tick()
    {
        var player = _detector.Player;

        // взгляд в зафиксированную позицию

        _animator.LookAt(_lockedTargetPosition);

        // если игрок совсем убежал далеко — можно выйти в погоню
        if (player != null)
        {
            float distNow = Vector3.Distance(player.position, _animator.Transform.position);
            if (distNow > _stats.AttackRange)
            {
                _machine.SetState(_factory.CreateChaseState());
                return;
            }
        }

        _vocalTimer -= Time.deltaTime;
        if (_vocalTimer <= 0f)
        {
            _sound.PlayAttack();
            _vocalTimer = _sound.GetRandomAttackInterval();
        }



        _attackCooldown -= Time.deltaTime;

        if (_attackCooldown <= 0f && !_animator.IsPlayingAttack())
        {
            _animator.PlayAttack();
            _attacksDone++;
            _attackCooldown = Random.Range(0.8f, 1.6f);


            if (_attacksDone >= _attacksInBurst)
            {
                return;

            }
        }
    }

    public void Exit()
    {
        _machine.AttackIntent = false;

        _animator.SetRootMotion(false);

        // выравнивание NavMeshAgent под фактическую позицию модели
        _agent.Warp(_animator.Transform.position);

        _agent.updatePosition = true;
        _agent.updateRotation = true;
        _agent.isStopped = false;
    }
}