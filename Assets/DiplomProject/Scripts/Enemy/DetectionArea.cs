using UnityEngine;
using System;

public class DetectionArea : MonoBehaviour
{
    public event Action<Transform> PlayerEntered;
    public event Action PlayerExited;
    private Transform _target;

    public Transform GetCurrentTarget()
    {
        return _target; // ��� ����� ������ �������� ������, ���� �� ��� � ����
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