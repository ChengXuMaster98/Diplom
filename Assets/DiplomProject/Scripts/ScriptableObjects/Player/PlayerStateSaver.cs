using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStateSaver", menuName = "ScriptableObjects/PlayerStateSaver", order = 1)]
public class PlayerStateSaver : ScriptableObject
{
    public int _currentHealth = 100;
    public int currentDamage = 10;

    public void Save(int health, int damage)
    {
        _currentHealth = health;
        currentDamage = damage;
    }
}