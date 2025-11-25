using UnityEngine;
using Zenject;

public class FirstPersonController : ITickable
{
    private readonly IInputService _input;
    private readonly CharacterController _controller;
    private readonly Transform _cameraTransform;
    private readonly Transform _bodyTransform;
    private readonly PlayerStats _stats;

    private readonly IPauseService _pauseService;

    private float _xRotation = 0f;
    //private float _yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;


    public FirstPersonController(
        IInputService input,
        Player player,
        PlayerStats stats,
        IPauseService pauseService)
    {
        _input = input;
        _controller = player.Controller;
        _cameraTransform = player.CameraTransform;
        _bodyTransform = player.BodyTransform;
        _stats = stats;
        _pauseService = pauseService;
    }

    public void Tick()
    {
        if (_pauseService.IsPaused)
            return;

        RotateCamera();
    }

    private void RotateCamera()
    {
        //Getting the mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * _stats.mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _stats.mouseSensitivity * Time.deltaTime;

        //Rotation around the x axis (Look up and down)
        _xRotation -= mouseY;
        
        //Clamp the rotation
        _xRotation = Mathf.Clamp(_xRotation, topClamp, bottomClamp);

        // Вращаем КАМЕРУ только по X (вверх-вниз)
        _cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

        // Вращаем ТЕЛО игрока по Y (влево-вправо)
        _bodyTransform.Rotate(Vector3.up * mouseX);

    }
}