using UnityEngine;
using Zenject;
using Cinemachine;

public class ThirdPersonCameraController : ITickable
{
    private readonly CinemachineFreeLook _camera;
    private readonly Transform _target;
    private readonly float _sensitivity;

    private float _xRotation;
    private float _yRotation;

    private const float TopClamp = -50f;
    private const float BottomClamp = 50f;

    public ThirdPersonCameraController(CinemachineFreeLook camera, Transform cameraTarget, PlayerStats stats)
    {
        _camera = camera;
        _target = cameraTarget;
        _sensitivity = stats.mouseSensitivity;

        _camera.Follow = _target;
        _camera.LookAt = _target;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Tick()
    {

    }
}