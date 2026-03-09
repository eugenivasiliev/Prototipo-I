using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultTower : Tower
{
    [SerializeField] private float damage;
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
                GetClosestValidEnemy();

            SpawnProjectile(waitTime);

            yield return new WaitForSeconds(waitTime);

            if (targetedEnemy != null)
                DamageTarget();
            else
            {
                GetClosestValidEnemy();
                DamageTarget();
            }
        }

        attacking = false;
    }

    void GetClosestValidEnemy()
    {
        closeEnemies.RemoveAll(item => item == null);

        targetedEnemy = null;
        attacking = false;

        if (closeEnemies.Count == 0) return;

        foreach (var e in closeEnemies)
        {
            if (Vector3.Distance(this.transform.position, e.transform.position) >= minRange)
            {
                targetedEnemy = e;
                attacking = true;
                return;
            }
        }
    }

    void DamageTarget()
    {
        if (targetedEnemy == null) return;

        if (targetedEnemy.TryGetComponent<IDamageable>(out var damageable)) { 
            damageable.DamagePercent(damage);

            targetedEnemy.GetComponent<EnemyAI>().UpdateLife();
        }
    }

    void Update() {
        if (!attacking)
            return;

        if (targetedEnemy == null)
        {
            GetClosestValidEnemy();
            return;
        }

        if (!tracking)
            return;


        Vector3 dir = targetedEnemy.transform.position - transform.position;
        Quaternion qt = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, qt, speed);
    }

    void SpawnProjectile(float waitTime)
    {
        
        AudioManager.instance.PlaySFX("TurretAttack");
        GameObject p = Instantiate(projectile, this.transform.position, this.transform.rotation);        
        p.GetComponent<Projectile>().startPos = transform.position;
        p.GetComponent<Projectile>().finalPos = targetedEnemy.transform;
        p.GetComponent<Projectile>().maxTime = waitTime;
    }
}
