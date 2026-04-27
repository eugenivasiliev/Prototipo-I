using System.Collections.Generic;
using Combat;
using Enemies;
using Objectives;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using System.Collections;

namespace TowerDefense
{
    public class Base : MonoBehaviour, IDamageable
    {

        [SerializeField] private int health;
        [SerializeField] private int maxHealth = 100;

        [SerializeField] private Image healthBar;
        [SerializeField] private MeshRenderer meshHolder;
        [SerializeField] private Material blinkMaterial;
        [SerializeField] private Material naturalMaterial;
        [SerializeField] private float blinkTime;
        public int Health { get => health; set => health = value; }
        public int MaxHealth { get => maxHealth; set => maxHealth = value; }
        [SerializeField] private Canvas ui_health;

        [SerializeField] private int seedsPerRound;

        private System.Action<float> BaseProduction;

        [SerializeField] private EnemyManager enemyManager;

        private void Start()
        {
            health = MaxHealth;
            BaseProduction += AddSeeds;
            DayNightCycle.Instance.SubscribeTimedEvent(BaseProduction, 2);
        }

        void Update()
        {
            if (((IDamageable)this).IsDead())
            {

                enemyManager.ReturnToSpawn();
                health = MaxHealth;
            }


        }

        void AddSeeds(float ff)
        {
            Inventory.Inventory.Instance.AddSeeds(seedsPerRound);
            if (ObjectivesManager.Instance.TryGetObjective<PlantsCollected, int>(out List<PlantsCollected> objs))
                foreach (PlantsCollected obj in objs) obj.UpdateObjective(seedsPerRound);
            DayNightCycle.Instance.SubscribeTimedEvent(BaseProduction, 2);
        }

        public void OnDamage() {

            healthBar.fillAmount = Health / MaxHealth;

            StartCoroutine(RedBlink());
        }

        IEnumerator RedBlink() {
            meshHolder.material = blinkMaterial;
            yield return new WaitForSeconds(blinkTime);
            meshHolder.material = naturalMaterial;
        }

    }
}