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
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;

        _yRotation -= mouseX;
        _xRotation += mouseY;
        _xRotation = Mathf.Clamp(_xRotation, TopClamp, BottomClamp);

        _target.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }
}