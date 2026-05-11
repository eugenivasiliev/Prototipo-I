using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.ProBuilder;

namespace Utils
{
    public class ContextKey : MonoBehaviour
    {
        [SerializeField] private GameObject contexted;
        [SerializeField] private GameObject key;
        [SerializeField] private GameObject controllerKey;

        enum WhichInput
        {
            KEYBOARD,
            CONTROLLER
        }

        private WhichInput wi = WhichInput.KEYBOARD;


        private void Update()
        {
            //keyboard keys & Mouse
            if (Input.anyKey)
            {
                wi = WhichInput.KEYBOARD;
                Debug.Log("Keyboard!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }
            //joystick
            else if (!Input.anyKey && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
            {
                wi = WhichInput.CONTROLLER;
                Debug.Log("Controller!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }

            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (contexted.GetComponent<IContexted>().ContextKeyActive() && other.tag == "Player")
            {
                switch (wi)
                {
                    case WhichInput.CONTROLLER:
                        controllerKey.SetActive(true);
                        break;
                    
                    case WhichInput.KEYBOARD:
                        key.SetActive(true);
                        break;

                    default:
                        break;
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                key.SetActive(false);
                controllerKey.SetActive(false);
            }
        }
    }
}