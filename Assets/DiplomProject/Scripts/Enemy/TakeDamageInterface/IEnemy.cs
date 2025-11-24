using UnityEngine;

public interface IEnemy: IDamageOverTime, IStunnable
{
    void TakeDamage(int damage);

    Vector3 CenterPoint { get; }
}