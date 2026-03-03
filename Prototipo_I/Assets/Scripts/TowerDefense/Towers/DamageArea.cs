using System.Collections;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(SelfDestruct());
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var enemy))
            enemy.DamageMax();
    }

    IEnumerator SelfDestruct() {

        yield return new WaitForSeconds(0.1f) ;
        Destroy(gameObject);
    }
}
