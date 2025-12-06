using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : IPlayerState
{
    private static readonly int Dash = Animator.StringToHash("Dash");

    private readonly Animator _animator;
    private readonly CharacterMovementController _movement;
    private readonly IPlayerStaminaConsumer _stamina;
    private readonly PlayerStateMachine _stateMachine;
    private readonly Player _player;

    private readonly Cinemachine.CinemachineVirtualCamera _camera;

    private bool _completed = false;
    private Coroutine _routine;


    private const float DashDuration = 0.30f;
    private const float DashDistance = 5.5f;
    private const float EaseOutTime = 0.20f;


    private Vector3 _dashDirection;

    public PlayerDashState(
        Animator animator,
        CharacterMovementController movement,
        IPlayerStaminaConsumer stamina,
        PlayerStateMachine stateMachine,
        Player player,
        Cinemachine.CinemachineVirtualCamera camera)
    {
        _animator = animator;
        _movement = movement;
        _stamina = stamina;
        _stateMachine = stateMachine;
        _player = player;
        _camera = camera;
    }

    public void Enter()
    {
        _completed = false;

        if (!_stamina.CanDash())
        {

            Debug.Log("Not enough stamina for dash");

            //_stateMachine.RevertToPreviousState();
            _completed = true;    // ← разрешаем немедленный выход из DashState
            return;

        }

        _stamina.ConsumeStaminaForDash();
        _animator.SetTrigger(Dash);

        _dashDirection = CalculateDashDirection();

        _routine = _player.StartCoroutine(DashRoutine());
    }

    private Vector3 CalculateDashDirection()
    {
        Transform cam = Camera.main.transform;

        Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight   = Vector3.Scale(cam.right, new Vector3(1, 0, 1)).normalized;

        if (Input.GetKey(KeyCode.A)) return -camRight;
        if (Input.GetKey(KeyCode.D)) return camRight;
        if (Input.GetKey(KeyCode.S)) return -camForward;

        return camForward;
    }

    private IEnumerator DashRoutine()
    {
        float t = 0f;

        float defaultFov = _camera.m_Lens.FieldOfView;
        float dashFov = defaultFov + 12f;
        float shakeAmplitude = 1.5f;
        float shakeFrequency = 2f;

        var noise = _camera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

        if (noise != null)
        {
            noise.m_AmplitudeGain = shakeAmplitude;
            noise.m_FrequencyGain = shakeFrequency;
        }

        float dashSpeed = DashDistance / DashDuration;


        while (t < DashDuration)
        {
            t += Time.deltaTime;
            float k = t / DashDuration;

            // Вход дэша
            float eased = k * k;


            _movement.Controller.Move(_dashDirection * (dashSpeed * eased) * Time.deltaTime);

            // Плавно увеличивается фов
            _camera.m_Lens.FieldOfView = Mathf.Lerp(defaultFov, dashFov, eased);

            yield return null;
        }

        // Инерция и плавный выход из дэша
        float inertia = 0f;
        float inertiaSpeed = dashSpeed * 0.35f;

        while (inertia < EaseOutTime)
        {
            inertia += Time.deltaTime;

            float k = inertia / EaseOutTime;
            float easedOut = 1f - (k * k);

            _movement.Controller.Move(_dashDirection * (inertiaSpeed * easedOut) * Time.deltaTime);

            // Здесь фов возвращается уже назад
            _camera.m_Lens.FieldOfView = Mathf.Lerp(dashFov, defaultFov, k);

            // И тряска плавно ослабляется, хэдбобинг чтоли
            if (noise != null)
                noise.m_AmplitudeGain = Mathf.Lerp(shakeAmplitude, 0f, k);

            yield return null;
        }

        // полная очистка тряски
        if (noise != null)
        {
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = shakeFrequency;
        }

        _camera.m_Lens.FieldOfView = defaultFov;

        _completed = true;
    }

    public void Tick() { }

    public bool CanExit() => _completed;

    public void Exit()
    {
        if (_routine != null)
            _player.StopCoroutine(_routine);
    }
}