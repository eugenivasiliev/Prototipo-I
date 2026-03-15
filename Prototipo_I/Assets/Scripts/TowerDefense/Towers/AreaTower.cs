using UnityEngine;
using UnityEngine.ProBuilder;
using System.Collections.Generic;
using System.Collections;

namespace TowerDefense
{
    public class AreaTower : Tower
    {

        private void Start()
        {
            waitTime = 1.5f;
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

            animator.SetBool("Shooting", true);

            while (attacking && closeEnemies.Count > 0)
            {
                if (targetedEnemy == null)
                    GetClosestValidEnemy();

                SpawnProjectile(waitTime);

                yield return new WaitForSeconds(waitTime);

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

            Vector3 fwd = new Vector3(this.transform.forward.x, 0, this.transform.forward.z);
            Vector3 enemyFwd = (targetedEnemy.transform.position - this.transform.position).normalized;
            Vector3 targetFwd = new Vector3(enemyFwd.x, 0, enemyFwd.z);

            Quaternion qt = Quaternion.FromToRotation(fwd, targetFwd);
            transform.rotation *= qt;
        }

        void SpawnProjectile(float waitTime)
        {
            GameObject p = Instantiate(projectile, this.transform.position, this.transform.rotation);
            p.GetComponent<Bomb>().startPos = transform.position;
            p.GetComponent<Bomb>().finalPos = targetedEnemy.transform;
            p.GetComponent<Bomb>().maxTime = waitTime;
        }
    }
}