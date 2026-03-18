using System;
using System.Collections.Generic;
using Audio;
using Enemies;
using UnityEngine;

namespace TowerDefense
{
    public class Tower : MonoBehaviour
    {
        [Header("Statistics")]
        [SerializeField] protected GameObject projectile;
        [SerializeField] protected bool tracking = true;
        [SerializeField, Range(0, 5)] protected float cooldown = 0.6f;
        [SerializeField, Range(0, 50)] protected float maxRange = 15;
        [SerializeField, Range(0, 50)] protected float minRange = 3;

        [Header("Animation")]
        [SerializeField] protected string shotSound = "TurretAttack";
        [SerializeField] protected Animator animator;
        [SerializeField] protected GameObject particles;
        [SerializeField] protected GameObject particlesOrigin;

        protected float currentCooldown = 0;
        protected bool attacking = false;
        protected List<GameObject> closeEnemies = new List<GameObject>();
        protected GameObject targetedEnemy;

        public float GetRange() => maxRange;

        protected virtual bool CanAttack() => true;

        protected virtual void SpawnProjectile()
        {
            AudioManager.Instance.PlaySFX(shotSound);
            Projectile projectileInstance = 
                Instantiate(projectile, this.transform.position, this.transform.rotation).GetComponent<Projectile>();
            projectileInstance.startPos = transform.position;
            projectileInstance.target = targetedEnemy;

            Instantiate(particles, particlesOrigin.transform.position, this.transform.rotation);
        }

        protected void GetClosestValidEnemy()
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

        protected void Update()
        {
            if (targetedEnemy == null)
                GetClosestValidEnemy();

            if (!attacking || !CanAttack() || targetedEnemy == null)
            {
                attacking = false;
                animator.SetBool("Shooting", false);
                return;
            }

            if (tracking) LookAtPivoted();

            currentCooldown += Time.deltaTime;
            if (currentCooldown < cooldown) return;

            LookAtPivoted();
            SpawnProjectile();

            currentCooldown = 0;
        }

        protected void LookAtPivoted()
        {
            Vector3 fwd = new Vector3(this.transform.forward.x, 0, this.transform.forward.z);
            Vector3 enemyFwd = (targetedEnemy.transform.position - this.transform.position).normalized;
            Vector3 targetFwd = new Vector3(enemyFwd.x, 0, enemyFwd.z);

            Quaternion qt = Quaternion.FromToRotation(fwd, targetFwd);
            transform.rotation *= qt;
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent<EnemyAI>(out var enemy)) return;

            closeEnemies.Add(other.gameObject);

            if (!attacking)
            {
                attacking = true;
                animator.SetBool("Shooting", true);
            }
        }

        protected void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.TryGetComponent<EnemyAI>(out var enemy)) return;

            closeEnemies.Remove(other.gameObject);

            if (closeEnemies.Count == 0)
            {
                attacking = false;
                targetedEnemy = null;
                animator.SetBool("Shooting", false);
            }
        }
    }
}