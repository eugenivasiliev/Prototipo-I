using System.Collections.Generic;
using Combat;
using Enemies;
using Objectives;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace TowerDefense
{
    public class Base : MonoBehaviour, IDamageable
    {
        public static Base instance;

        [SerializeField] private int health;

        [SerializeField] private Image healthBar;

        public int Health { get => health; set => health = value; }
        public int MaxHealth { get => 100; set { } }
        [SerializeField] private Canvas ui_health;

        [SerializeField] private int seedsPerRound;

        private System.Action<float> BaseProduction;

        [SerializeField] private EnemyManager enemyManager;

        private void Start()
        {
            health = MaxHealth;
            instance = this;
            BaseProduction += AddSeeds;
            DayNightCycle.Instance.SubscribeTimedEvent(BaseProduction, 2);
        }

        void Update()
        {
            if (health <= 0.0f)
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
        }
    }
}