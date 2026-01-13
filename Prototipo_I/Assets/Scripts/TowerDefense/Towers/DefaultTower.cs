using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultTower : MonoBehaviour
{
    bool attacking = false;
    private List<GameObject> closeEnemies = new List<GameObject>();
    private GameObject targetedEnemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {

            closeEnemies.Add(other.gameObject);

            if (attacking == false)
                StartCoroutine(AttackLoop());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            closeEnemies.Remove(other.gameObject);

            if (closeEnemies.Count == 0)
            {

                attacking = false;
                targetedEnemy = null;
            }
        }
    }

    IEnumerator AttackLoop()
    {
        attacking = true;

        while (attacking && closeEnemies.Count > 0)
        {
            if (targetedEnemy == null)
                GetClosestEnemy();

            yield return new WaitForSeconds(0.6f);

            if (targetedEnemy != null)
                DamageTarget();
            else
            {
                GetClosestEnemy();
                DamageTarget();
            }
        }

        attacking = false;
    }

    void GetClosestEnemy()
    {
        closeEnemies.RemoveAll(item => item == null);

        if (closeEnemies.Count > 0)
            targetedEnemy = closeEnemies[0];
        else
        {
            targetedEnemy = null;
            attacking = false;
        }
    }

    void DamageTarget()
    {
        if (targetedEnemy == null) return;

        if (targetedEnemy.TryGetComponent<IDamageable>(out var damageable))
            damageable.DamageMax();
    }
}
