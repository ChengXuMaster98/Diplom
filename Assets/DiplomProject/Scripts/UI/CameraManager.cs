using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Virtual Cameras")]
    public CinemachineVirtualCamera firstPersonVCam;
    public CinemachineFreeLook thirdPersonVCam;

    [Header("Real Cameras")]
    public Camera mainCamera;
    public Camera weaponCamera;

    [Header("Culling Masks")]
    public LayerMask fpvMask;     // Всё кроме оружия
    public LayerMask tppMask;     // Всё (или всё кроме FPS-рук, если будут)
    public LayerMask weaponMask;  // Только оружие

    private bool _isFirstPerson = true;

    public bool IsFirstPerson => _isFirstPerson;

    void Start()
    {
        // ВСЕГДА корректно включаем состояние начальной камеры
        SetFirstPerson(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleCamera();
        }
    }

    private void ToggleCamera()
    {
        SetFirstPerson(!_isFirstPerson);
    }

    private void SetFirstPerson(bool value)
    {
        _isFirstPerson = value;

        if (_isFirstPerson)
        {
            // FIRST PERSON
            firstPersonVCam.Priority = 20;
            thirdPersonVCam.Priority = 0;

            weaponCamera.enabled = true;
            weaponCamera.cullingMask = weaponMask;

            mainCamera.cullingMask = fpvMask;
        }
        else
        {
            // THIRD PERSON
            firstPersonVCam.Priority = 0;
            thirdPersonVCam.Priority = 20;

            weaponCamera.enabled = false; // ВАЖНО!

            mainCamera.cullingMask = tppMask;
        }
    }
}