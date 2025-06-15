using UnityEngine;
using Zenject;

public class PlayerAttackHandler : MonoBehaviour
{
    [SerializeField] private Collider _weaponCollider;

    private PlayerStats _stats;

    [Inject]
    public void Construct(PlayerStats stats)
    {
        _stats = stats;
    }

    private void Awake()
    {
        _weaponCollider.enabled = false;
    }

    public void EnableWeaponCollider()
    {
        _weaponCollider.enabled = true;
    }

    public void DisableWeaponCollider()
    {
        _weaponCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyHealth>(out var enemyHealth))
        {
            enemyHealth.TakeDamage(_stats.attackDamage);
        }
    }
}
