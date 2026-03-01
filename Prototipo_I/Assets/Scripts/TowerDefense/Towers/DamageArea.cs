using System.Collections;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<EnemyAI>(out var enemy))
            enemy.GetFrozen();
    }

    IEnumerator SelfDestruct() {


        yield return new WaitForSeconds(0.1f) ;

    }
}
