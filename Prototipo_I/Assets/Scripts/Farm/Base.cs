using Combat;
using Enemies;
using Items;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace TowerDefense
{
    public class Base : MonoBehaviour, IDamageable
    {
        public static Base instance;

        [SerializeField] private int health;

        public int Health { get => health; set => health = value; }
        public int MaxHealth { get => 100; set { } }
        [SerializeField] private Canvas ui_health;

        [SerializeField] private int seedsPerRound;

        private UnityEvent<float> BaseProduction = new UnityEvent<float>();
        private void Start()
        {
            health = MaxHealth;
            instance = this;
            BaseProduction.AddListener(AddSeeds);
            DayNightCycle.Instance.SubscribeTimedEvent(BaseProduction, 1);
        }

        void Update()
        {
            if (health <= 0.0f)
            {

                EnemyManager.Instance.ReturnToSpawn(0.0f);
                health = MaxHealth;
            }
        }

        void AddSeeds(float ff)
        {
            Inventory.Inventory.Instance.AddSeeds(seedsPerRound);
            DayNightCycle.Instance.SubscribeTimedEvent(BaseProduction, 1);
        }


    }
}