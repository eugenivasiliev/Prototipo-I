using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerCursorMovement : MonoBehaviour
{
    private int speed = 7;

    private void Update()
    {
        if (Gamepad.current == null)
            return;

        if (Gamepad.current.leftStick.ReadValue() == Vector2.zero)
            return;

        Vector2 newPos = Mouse.current.position.ReadValue() +
            Gamepad.current.leftStick.ReadValue() * speed;

        Mouse.current.WarpCursorPosition(newPos);
    }
}
