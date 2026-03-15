using UnityEngine;

namespace HelperBox
{
    public class HelperBox : MonoBehaviour
    {
        [SerializeField] private int seedsAdded = 60;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Inventory.Inventory.Instance.AddSeeds(seedsAdded);
                Destroy(gameObject);
            }
        }
    }
}