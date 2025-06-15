using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyStats _stats;
    private float _currentHealth;

    public void Initialize(EnemyStats stats)
    {
        Debug.Log($"[{gameObject.name}] ����� � HP = {_currentHealth}");
        _stats = stats;
        _currentHealth = _stats.MaxHealth;
    }

    public void TakeDamage(float amount)
    {
        Debug.Log($"[{gameObject.name}] ������� ����: {amount}");
        _currentHealth -= amount;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died.");
    }
}