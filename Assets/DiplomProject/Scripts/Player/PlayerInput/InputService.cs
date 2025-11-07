using UnityEngine;

public class InputService : IInputService
{
    public Vector2 MoveInput => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    public Vector2 LookInput => new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
}