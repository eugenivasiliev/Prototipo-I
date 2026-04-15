using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ControllerButton : MonoBehaviour
{
    [SerializeField] private GameObject myButton;
    [SerializeField] private GameObject previousButton;
    void OnAwake()
    {
        SetButton(myButton);
    }

    private void OnEnable()
    {
        SetButton(myButton);
    }

    private void OnDisable()
    {
        SetButton(previousButton);
    }

    void SetButton(GameObject button)
    {

        Gamepad controller = Gamepad.current;

        //if (controller != null)
            EventSystem.current.SetSelectedGameObject(button);
    }
}
