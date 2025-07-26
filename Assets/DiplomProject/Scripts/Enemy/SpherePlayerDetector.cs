using UnityEngine;
using Zenject;
using System;

public class SpherePlayerDetector : MonoBehaviour, IPlayerDetector, IInitializable
{
    //[SerializeField] private float detectionRadius = 10f;
    //[SerializeField] private LayerMask playerLayer;

    public event Action<Transform> PlayerDetected;
    public event Action PlayerLost;

    private float _detectionRadius;
    private LayerMask _playerMask;

    private Transform _player;

    public Transform Player => _player;
    private bool _isPlayerInRange;

    private EnemyStats _stats;

    [Inject]
    public void Construct(EnemyStats stats)
    {
        _stats = stats;
    }

    public void Initialize()
    {
        _detectionRadius = _stats.DetectionRadius;
        _playerMask = _stats.PlayerMask;
    }

    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius, _playerMask);
        bool playerFound = false;

        foreach (var hit in hits)
        {
            Debug.Log(hits.Length);
            Debug.Log(hit.name);

            if (hit.CompareTag("Player"))
            {
                playerFound = true;
                if (!_isPlayerInRange)
                {
                    _isPlayerInRange = true;
                    _player = hit.transform;
                    PlayerDetected?.Invoke(_player);
                    Debug.Log(">> PlayerDetected invoked with: " + _player.name);
                }
                break;
            }
        }

        if (!playerFound && _isPlayerInRange)
        {
            _isPlayerInRange = false;
            _player = null;
            PlayerLost?.Invoke();
        }

    }
}