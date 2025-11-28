using UnityEngine;

public interface IInputService
{
    Vector2 MoveInput { get; }
    Vector2 LookInput { get; }

    void EnableInput();
    void DisableInput();

    bool IsInputEnabled { get; }

    bool TabPressedThisFrame();
}