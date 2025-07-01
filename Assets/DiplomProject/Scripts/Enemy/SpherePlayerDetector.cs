using UnityEngine;
using System;
using Zenject;

public class SpherePlayerDetector : MonoBehaviour, IPlayerDetector, ITickable
{
    private readonly float _detectionRadius;
    private readonly LayerMask _playerMask;
    private Transform _lastDetectedPlayer;

    public event Action<Transform> PlayerDetected;
    public event Action PlayerLost;
    public Transform LastKnownPlayer => _lastDetectedPlayer;

    public SpherePlayerDetector(EnemyStats stats)
    {
        _detectionRadius = stats.DetectionRadius;
        _playerMask = LayerMask.GetMask("Player");
    }

    public void Tick()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius, _playerMask);

        if (hits.Length > 0)
        {
            if (_lastDetectedPlayer == null)
            {
                _lastDetectedPlayer = hits[0].transform;
                PlayerDetected?.Invoke(_lastDetectedPlayer);
            }
        }
        else
        {
            if (_lastDetectedPlayer != null)
            {
                _lastDetectedPlayer = null;
                PlayerLost?.Invoke();
            }
        }
    }
}