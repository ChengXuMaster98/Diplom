using System;
using Unity.VisualScripting;
using UnityEngine;

public class SpherePlayerDetector : MonoBehaviour, IPlayerDetector
{
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private LayerMask playerLayer;

    public event Action<Transform> PlayerDetected;
    public event Action PlayerLost;

    private Transform _player;
    private bool _isPlayerInRange;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
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