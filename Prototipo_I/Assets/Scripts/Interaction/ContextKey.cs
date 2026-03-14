using UnityEngine;
using UnityEngine.ProBuilder;

namespace Utils
{
    public class ContextKey : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {

                transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}