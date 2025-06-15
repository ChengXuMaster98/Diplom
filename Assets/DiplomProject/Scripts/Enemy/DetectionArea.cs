using UnityEngine;
using System;

public class DetectionArea : MonoBehaviour
{
    public event Action<Transform> PlayerEntered;
    public event Action PlayerExited;
    private Transform _target;

    public Transform GetCurrentTarget()
    {
        return _target; // или любой способ получить игрока, если он уже в зоне
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered: " + other.name);
        if (other.CompareTag("Player"))
            PlayerEntered?.Invoke(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            PlayerExited?.Invoke();
    }
}