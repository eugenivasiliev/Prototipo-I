using System.Collections;
using System.Collections.Generic;
using Audio;
using Combat;
using Enemies;
using Farm;
using Inventory;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace TowerDefense
{
    public class PlantBasedTower : Tower, IInteractable, IContexted
    {

        [SerializeField] private int damage;
        [SerializeField] private (PlantData data, float amount) currentPlant = (null, 0f);
        private readonly int maxCapacity = 3;
        private readonly float usesPerAttack = .2f;
        private bool CanAttack => currentPlant.amount >= usesPerAttack;

        public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding> {
            new IInteractable.KeyBinding("refill_tower", InputActionChange.ActionCanceled, Action_Refill)
        };

        private void Start()
        {
            (this as IInteractable).Bind();
        }

        void Action_Refill(InputAction.CallbackContext context)
        {
            if(Inventory.Inventory.Instance.GetSeedCount() > maxCapacity - (int)Mathf.Floor(currentPlant.amount))
            {
                Inventory.Inventory.Instance.RemoveSeeds(maxCapacity - (int)Mathf.Floor(currentPlant.amount));
                currentPlant = (null, maxCapacity);
            }
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.TryGetComponent<EnemyAI>(out var enemy))
            {

                closeEnemies.Add(other.gameObject);

                if (attacking == false)
                    StartCoroutine(AttackLoop());
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


            while (attacking && closeEnemies.Count > 0 && CanAttack)
            {
                if (targetedEnemy == null)
                    GetClosestEnemy();

                SpawnProjectile(waitTime);

                yield return new WaitForSeconds(waitTime);
            }

            attacking = false;

            animator.SetBool("Shooting", false);
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

        public void OnInteract()
        {
            throw new System.NotImplementedException();
        }

        void SpawnProjectile(float waitTime)
        {
            Vector3 fwd = new Vector3(this.transform.forward.x, 0, this.transform.forward.z);
            Vector3 enemyFwd = (targetedEnemy.transform.position - this.transform.position).normalized;
            Vector3 targetFwd = new Vector3(enemyFwd.x, 0, enemyFwd.z);

            Quaternion qt = Quaternion.FromToRotation(fwd, targetFwd);
            transform.rotation *= qt;

            AudioManager.Instance.PlaySFX("TurretVAttack");
            GameObject p = Instantiate(projectile, this.transform.position, this.transform.rotation);
            p.transform.rotation = Quaternion.Euler(180, p.transform.rotation.eulerAngles.y, p.transform.rotation.eulerAngles.z);
            p.GetComponent<Projectile>().startPos = transform.position;
            p.GetComponent<Projectile>().target = targetedEnemy;
        }

        public bool ContextKeyActive() => !CanAttack;
    }
}