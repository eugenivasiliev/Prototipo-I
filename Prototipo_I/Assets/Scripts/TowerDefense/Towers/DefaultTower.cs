using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultTower : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    bool attacking = false;
    private List<GameObject> closeEnemies = new List<GameObject>();
    private GameObject targetedEnemy;

    private bool tracking = true;
    private float speed  = 4.5f;
    float waitTime = 0.6f;
    private float range = 15;

    public float GetRange() { 
        return range;
    }

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

            SpawnProjectile(waitTime);

            yield return new WaitForSeconds(waitTime);

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

        if (targetedEnemy.TryGetComponent<IDamageable>(out var damageable)) { 
            damageable.DamagePercent(20.0f);

            targetedEnemy.GetComponent<EnemyAI>().UpdateLife();
        }
    }

    void Update() {
        if (!attacking)
            return;

        if (targetedEnemy == null)
        {
            GetClosestEnemy();
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
