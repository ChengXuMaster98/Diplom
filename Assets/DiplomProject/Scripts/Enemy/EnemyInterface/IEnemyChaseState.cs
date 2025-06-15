using UnityEngine;
public interface IEnemyChaseState : IEnemyState
{
    void SetTarget(Transform target);
}
