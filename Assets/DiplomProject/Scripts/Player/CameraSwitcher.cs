using UnityEngine;
using Cinemachine;
using Zenject;
using System.Threading;

public class CameraSwitcher : ITickable
{
    private readonly CinemachineFreeLook _freeLookCamera;
    private readonly CinemachineVirtualCamera _fpsCamera;
    private readonly Renderer[] _headRenderers;

    private bool _isFirstPerson;

    public CameraSwitcher(CinemachineFreeLook freeLook, CinemachineVirtualCamera fpsCamera, Transform head)
    {
        _freeLookCamera = freeLook;
        _fpsCamera = fpsCamera;
        _headRenderers = head.GetComponentsInChildren<Renderer>();
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            _isFirstPerson = !_isFirstPerson;
            Debug.Log($"Switching camera: FirstPerson = {_isFirstPerson}");
            SwitchCamera();
        }
    }

    private void SwitchCamera()
    {
        _freeLookCamera.Priority = _isFirstPerson ? 5 : 10;
        _fpsCamera.Priority = _isFirstPerson ? 10 : 5;

        foreach (var renderer in _headRenderers)
        {
            renderer.enabled = !_isFirstPerson;
        }
    }
}
