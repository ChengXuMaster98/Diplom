using UnityEngine;
using Zenject;
using System;

public class SpherePlayerDetector : MonoBehaviour, IPlayerDetector, IInitializable
{

    public event Action<Transform> PlayerDetected;
    public event Action PlayerLost;

    private float _detectionRadius;
    private LayerMask _playerMask;

    private Transform _player;

    public Transform Player => _player;
    private bool _isPlayerInRange;

    private EnemyStats _stats;

    [SerializeField]
    private float _checkInterval = 0.2f; // как часто проверяем (сек)

    private float _nextCheckTime;

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

        if (Time.time < _nextCheckTime)
            return;

        _nextCheckTime = Time.time + _checkInterval;
        CheckPlayer();

    }

    private void CheckPlayer()
    {
        bool playerFound = false;

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            _detectionRadius,
            _playerMask);

        for (int i = 0; i < hits.Length; i++)
        {
            var hit = hits[i];

            if (!hit.CompareTag("Player"))
                continue;

            playerFound = true;

            if (!_isPlayerInRange)
            {
                _isPlayerInRange = true;
                _player = hit.transform;
                PlayerDetected?.Invoke(_player);
                // DevLog.Log($">> PlayerDetected: {_player.name}");
            }

            break;
        }

        if (!playerFound && _isPlayerInRange)
        {
            _isPlayerInRange = false;
            _player = null;
            PlayerLost?.Invoke();
        }
    }
}