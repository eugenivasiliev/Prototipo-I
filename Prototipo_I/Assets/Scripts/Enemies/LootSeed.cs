using UnityEngine;

namespace Enemies
{
    public class LootSeed : MonoBehaviour
    {
        [SerializeField, Range(0, 500)] private float forceRange = 300f;
        [SerializeField] private GameObject collectionParticles;

        virtual protected void Start()
        {

            Vector3 randomForce = new Vector3(
                Random.Range(-forceRange, forceRange),
                forceRange,
                Random.Range(-forceRange, forceRange)
                );

            this.GetComponent<Rigidbody>().AddForce(randomForce);
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;

            Inventory.Inventory.Instance.AddSeeds(1);
            Instantiate(collectionParticles, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}