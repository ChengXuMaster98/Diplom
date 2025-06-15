using UnityEngine;
using Zenject;

public class PlayerAttackController : IPlayerAttacker
{
    private readonly Player _player;
    private readonly PlayerStats _stats;
    private readonly Collider _attackCollider;

    public PlayerAttackController(Player player, PlayerStats stats)
    {
        _player = player;
        _stats = stats;
        _attackCollider = _player.AttackCollider;
        _attackCollider.enabled = false;
    }

    public void Attack()
    {
        _attackCollider.enabled = true;
        _player.Animator.SetTrigger("Attack");
    }

    // Этот метод вызывается через событие в анимации атаки
    public void EnableHitbox() => _attackCollider.enabled = true;
    public void DisableHitbox() => _attackCollider.enabled = false;
}
