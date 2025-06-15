using UnityEngine;

public interface IEnemyFactory
{
    Enemy Create(EnemyType type, Vector3 position);
}