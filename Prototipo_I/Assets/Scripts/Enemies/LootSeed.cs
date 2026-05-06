using System.Collections;
using Player;
using UnityEngine;
using Utils;

namespace Enemies
{
    public class LootSeed : MonoBehaviour
    {
        [SerializeField, Range(0, 500)] private float forceRange = 300f;
        [SerializeField, Range(0, 5)] private float collectionWaitTime;
        [SerializeField] private Tween<Vector3> collectionTween;
        [SerializeField] private GameObject collectionParticles;

        virtual protected void Start()
        {

            Vector3 randomForce = new Vector3(
                Random.Range(-forceRange, forceRange),
                forceRange,
                Random.Range(-forceRange, forceRange)
                );

            this.GetComponent<Rigidbody>().AddForce(randomForce);

            StartCoroutine(Collection());
            
        }

        protected void Update()
        {
            if (TweenUtil.Update(Time.deltaTime, ref collectionTween))
                this.transform.position = collectionTween.value;

            if(collectionTween.t == collectionTween.duration)
            {
                Inventory.Inventory.Instance.AddSeeds(1);
                SeedPickupUI.Instance.SeedPickedUp();
                Instantiate(collectionParticles, this.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }

        protected IEnumerator Collection()
        {
            yield return new WaitForSeconds(collectionWaitTime);

            collectionTween.startValue = this.transform.position;
            collectionTween.endValue = PlayerController.Instance.transform.position;
            collectionTween.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            
            if ((transform.position - other.transform.position).sqrMagnitude > 5.0f) return;

            
        }
    }
}