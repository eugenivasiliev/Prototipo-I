using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class ControllerCursorMovement : MonoBehaviour
{
    private int speed = 7;

    private VirtualMouseInput virtualMouse;

    private void Start()
    {
        virtualMouse = GetComponent<VirtualMouseInput>();
    }

    private void Update()
    {
        if (Gamepad.current == null)
            return;

        if (Gamepad.current.leftStick.ReadValue() == Vector2.zero)
            return;

        Vector2 delta = Mouse.current.delta.ReadValue();
        if (delta.x > 0.01f || delta.y > 0.01f) {
            Vector2 currentPos = virtualMouse.cursorTransform.position;
            virtualMouse.cursorTransform.position = currentPos + delta;
        }


        Vector2 newPos = Mouse.current.position.ReadValue() +
            Gamepad.current.leftStick.ReadValue() * speed;

        Mouse.current.WarpCursorPosition(newPos);
    }
}
