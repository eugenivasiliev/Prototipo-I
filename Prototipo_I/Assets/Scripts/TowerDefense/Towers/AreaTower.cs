using UnityEngine;
using UnityEngine.ProBuilder;
using System.Collections.Generic;
using System.Collections;
using Enemies;

namespace TowerDefense
{
    public class AreaTower : Tower
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<EnemyAI>(out var enemy))
            {

                closeEnemies.Add(other.gameObject);

                if (attacking == false)
                {
                    attacking = true;
                    animator.SetBool("Shooting", true);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<EnemyAI>(out var enemy))
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

                SpawnProjectile();

                yield return new WaitForSeconds(cooldown);

            }

            attacking = false;

            animator.SetBool("Shooting", false);
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
    }
}