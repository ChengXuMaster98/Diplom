using UnityEngine;
using Zenject;

public class CharacterMovementController : ITickable
{
    private readonly PlayerStats _stats;
    private readonly CharacterController _controller;
    private readonly Transform _cameraTransform;
    private readonly Transform _modelTransform;

    private Vector3 _direction;

    [Inject]
    public CharacterMovementController(PlayerStats stats, Player player, Transform cameraTransform)
    {
        _stats = stats;
        _controller = player.Controller;
        _cameraTransform = cameraTransform;
        _modelTransform = player.ModelTransform;
    }

    public void Move(Vector2 input)
    {
        if(input.magnitude > 0.1f)
        {
            Vector3 camForward = Vector3.Scale(_cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 camRight = Vector3.Scale(_cameraTransform.right, new Vector3(1, 0, 1)).normalized;
            _direction = (camRight * input.x + camForward * input.y).normalized;
        }

        else
        {
            _direction = Vector3.zero;
        }
        //Debug.Log($"MoveDir: {_direction}, CamFwd: {_cameraTransform.forward}, CamRight: {_cameraTransform.right}");
    }

    public void Tick()
    {
        if (_direction.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            _modelTransform.rotation = Quaternion.Slerp(
                _modelTransform.rotation,
                targetRotation,
                _stats.RotationSpeed * Time.deltaTime
            );
        }

        _controller.Move(_direction * _stats.MoveSpeed * Time.deltaTime);
    }
}