using UnityEngine;
using System.Collections.Generic;

public class EnemyPatrolPath : MonoBehaviour
{
    [SerializeField] private List<Transform> _patrolPoints;
    [SerializeField] private float _speed = 2f;
    private int _currentIndex = 0;

    private void Start()
    {
        if (_patrolPoints.Count == 0)
            Debug.LogWarning("No patrol points set for enemy.");
    }

    public void MoveNext()
    {
        if (_patrolPoints.Count == 0) return;

        Transform target = _patrolPoints[_currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
            _currentIndex = (_currentIndex + 1) % _patrolPoints.Count;
    }
}
