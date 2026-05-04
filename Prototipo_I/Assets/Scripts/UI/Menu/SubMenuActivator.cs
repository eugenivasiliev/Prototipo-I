using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SubMenuActivator : MonoBehaviour
{
    [SerializeField] GameObject myButton;
    private void OnEnable()
    {
        if (Gamepad.current == null)
            return;
        EventSystem.current.SetSelectedGameObject(myButton);
    }
}
