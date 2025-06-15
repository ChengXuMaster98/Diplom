using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState", menuName = "Game/PlayerState")]
public class PlayerStateSaver : ScriptableObject
{
    public float CurrentHealth;
    public float CurrentDamage;
}