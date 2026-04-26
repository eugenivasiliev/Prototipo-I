using UnityEngine;

namespace Enemies
{
    public class LootSeed : MonoBehaviour
    {
        virtual protected void Start()
        {

            float number = 300f;

            Vector3 randomForce = new Vector3(Random.Range(-number, number),
                number,
                Random.Range(-number, number));

            this.GetComponent<Rigidbody>().AddForce(randomForce);
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Inventory.Inventory.Instance.AddSeeds(1);
                Destroy(this.gameObject);
            }
        }
    }
}