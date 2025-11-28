using UnityEngine;

public class InputService : IInputService
{
    private bool _enabled = true;

    public bool IsInputEnabled => _enabled;

    public void EnableInput()
    {
        _enabled = true;
    }

    public void DisableInput()
    {
        _enabled = false;
    }

    public Vector2 MoveInput =>
        _enabled
            ? new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            : Vector2.zero;

    public Vector2 LookInput =>
        _enabled
            ? new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))
            : Vector2.zero;

    public bool TabPressedThisFrame() =>
        _enabled && Input.GetKeyDown(KeyCode.Tab);
}