using Items;
using UnityEngine;

namespace HelperBox
{
    public class HelperBox : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Fire();
                Gas();
                Destroy(gameObject);
            }
        }

        private void Gas()
        {
            Inventory.Inventory.Instance.AddItem(new GasPlantItem(), 30, out int amountDone);
        }
        private void Fire()
        {
            Inventory.Inventory.Instance.AddItem(new FirePlantItem(), 30, out int amountDone);
        }
    }
}