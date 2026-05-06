using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;

namespace Utils
{
    public class ContextKey : MonoBehaviour
    {
        [SerializeField] private GameObject contexted;
        [SerializeField] private GameObject key;
        [SerializeField] private GameObject controllerKey;
        private void OnTriggerEnter(Collider other)
        {
            if (contexted.GetComponent<IContexted>().ContextKeyActive() && other.tag == "Player")
            {
                if (Gamepad.current != null)
                    controllerKey.SetActive(true);
                else 
                    key.SetActive(true);
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