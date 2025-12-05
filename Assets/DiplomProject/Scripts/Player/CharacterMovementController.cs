using UnityEngine;
using Zenject;

public class CharacterMovementController : ITickable
{
    private readonly PlayerStats _stats;
    private readonly CharacterController _controller;
    private readonly Transform _cameraTransform;
    private readonly Transform _bodyTransform;

    private readonly IUpgradeService _upgradeService;

    private Vector3 _direction;

    private bool _movementBlocked = false;

    public float groundDistance = 0.15f;

    private readonly LayerMask _groundMask;
    public Transform _groundCheck;

    private readonly IPauseService _pauseService;

    Vector3 velocity;

    public CharacterController Controller => _controller;

    bool isGrounded;
    bool isMoving;
    private bool _canJump;

    public bool IsGrounded => isGrounded;
    public float VerticalVelocity => velocity.y;


    public CharacterMovementController(PlayerStats stats, Player player, Transform cameraTarget, Transform groundCheck, LayerMask groundMask, IUpgradeService upgradeService, IPauseService pauseService)
    {
        _stats = stats;
        _controller = player.Controller;
        _cameraTransform = cameraTarget;
        _bodyTransform = player.BodyTransform;
        _groundCheck = groundCheck;
        _groundMask = groundMask;
        _upgradeService = upgradeService;
        _pauseService = pauseService;
    }

    public void Move(Vector2 input)
    {
        if (input.magnitude > 0.1f)
        {
            // Направление от камеры, а не от тела
            Vector3 camForward = Vector3.Scale(_cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 camRight = Vector3.Scale(_cameraTransform.right, new Vector3(1, 0, 1)).normalized;
            _direction = (camRight * input.x + camForward * input.y).normalized;
        }

        else
        {
            _direction = Vector3.zero;
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(_stats.JumpHeight * -2f * _stats.gravity);
        }
    }

    public void BlockMovement()
    {
        _movementBlocked = true;
    }

    public void UnblockMovement()
    {
        _movementBlocked = false;
    }

    public void ForceDash(Vector3 dir)
    {
        Vector3 dash = dir.normalized * 2f; // сила дэша
        _controller.Move(dash);
    }

    public void Tick()
    {
        if (_pauseService.IsPaused)
            return;

        if (_movementBlocked)
            return;

        isGrounded = Physics.CheckSphere(_groundCheck.position, groundDistance, _groundMask);

        if (isGrounded && velocity.y < 0)

        {
            velocity.y = 0f;
            _canJump = true;
        }

        // Falling down
        velocity.y += _stats.gravity * Time.deltaTime;

        //Executing the jump
        _controller.Move(velocity * Time.deltaTime);
        //_controller.Move(_direction * _stats.MoveSpeed * Time.deltaTime);

        float effectiveSpeed = _stats.MoveSpeed * _upgradeService.SpeedMultiplier;
        _controller.Move(_direction * effectiveSpeed * Time.deltaTime);

        HandleRotation();




        //Debug.Log($"Grounded: {isGrounded}, Velocity Y: {velocity.y}, Position Y: {_controller.transform.position.y}");
    }

    private void HandleRotation()
    {
        // Игрок поворачивается ТОЛЬКО при движении
        if (_direction.sqrMagnitude > 0.1f)
        {
            // Мгновенный поворот тела в сторону камеры
            Vector3 lookDirection = Vector3.Scale(_cameraTransform.forward, new Vector3(1, 0, 1)).normalized;

            _bodyTransform.rotation = Quaternion.Slerp(_bodyTransform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * _stats.RotationSpeed);
        }
    }
}
