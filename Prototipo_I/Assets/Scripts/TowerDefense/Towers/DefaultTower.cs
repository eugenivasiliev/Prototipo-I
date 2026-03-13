using System.Collections;
using System.Collections.Generic;
using Audio;
using Combat;
using Enemies;
using UnityEngine;

namespace TowerDefense
{
    public class DefaultTower : Tower
    {
        private float damage = 20.0f;
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

            animator.SetBool("Shooting", true);

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

            animator.SetBool("Shooting", false);
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

            if (targetedEnemy.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.DamagePercent(20.0f);

                targetedEnemy.GetComponent<EnemyAI>().UpdateLife();
            }
        }

        void Update()
        {
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

            AudioManager.Instance.PlaySFX("TurretAttack");
            GameObject p = Instantiate(projectile, this.transform.position, this.transform.rotation);
            p.GetComponent<Projectile>().startPos = transform.position;
            p.GetComponent<Projectile>().finalPos = targetedEnemy.transform;
            p.GetComponent<Projectile>().maxTime = waitTime;
        }
    }
}