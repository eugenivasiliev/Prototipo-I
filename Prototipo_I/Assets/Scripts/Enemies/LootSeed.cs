using UnityEngine;

namespace Enemies
{
    public class LootSeed : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Inventory.Inventory.Instance.AddSeeds(1);
                SeedPickupUI.Instance.SeedPickedUp();
                Destroy(this.gameObject);
            }
        }
    }
}