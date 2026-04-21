using UnityEngine;
using UnityEngine.ProBuilder;

namespace Utils
{
    public class ContextKey : MonoBehaviour
    {
        [SerializeField] private GameObject contexted;
        [SerializeField] private GameObject key;

        private void OnTriggerEnter(Collider other)
        {
            if (contexted.GetComponent<IContexted>().ContextKeyActive() && other.tag == "Player")
            {
                key.SetActive(true);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                key.SetActive(false);
            }
        }
    }
}