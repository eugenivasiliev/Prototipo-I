using System.Collections;
using UnityEngine;

namespace Combat
{
    public class DamageArea : MonoBehaviour
    {
        [SerializeField] int damage;
        private void Awake()
        {
            StartCoroutine(SelfDestruct());
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent<IDamageable>(out var enemy))
                enemy.Damage(damage);
        }

        IEnumerator SelfDestruct()
        {

            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }
    }
}