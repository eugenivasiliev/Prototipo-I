using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Combat;
using Farm;
using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Utils;

namespace Enemies
{
    public class EnemyAI : MonoBehaviour, IAttacker, IDamageable
    {
        [Serializable]
        public struct Blackboard
        {
            public Target target;
            public Transform targetTransform;
            public Transform homeTransform;
            public List<Plot> plots;
            public PlayerController playerController;
        }

        public enum Target
        {
            Home,
            Plots,
            Player
        }

        public enum State
        {
            Chase,
            Attack,
            Return
        }

        [SerializeField] private EnemyState enemyState;

        private NavMeshAgent agent;
        public NavMeshAgent Agent { get => agent; }

        [SerializeField] private int health;

        public int Health { get => health; set => health = value; }
        public int MaxHealth { get => 100; set { } }
        [SerializeField] private Canvas ui_health;


        [SerializeField] private int damage;
        public int Damage => damage;

        [SerializeField] private int difficulty;


        [SerializeField] private float speed;
        [SerializeField] private float slowSpeed;


        public int Difficulty => difficulty;

        [Serializable]
        public struct DropRateObject
        {
            public GameObject gameObject;
            public float rate;

            public DropRateObject(GameObject gameObject, float rate)
            {
                this.gameObject = gameObject;
                this.rate = rate;
            }
        }

        [Header("Loot")]
        [SerializeField] private List<DropRateObject> droppableLoot;
        [SerializeField] private int minItemsDropped;
        [SerializeField] private int maxItemsDropped;
        [SerializeField, Range(0, 5)] private float dropRadius;
        [SerializeField, Range(0, 5)] private float dropHeight = 2;

        [SerializeField] private Blackboard bb;
        public Blackboard BB { get => bb; set => bb = value; }


        bool frozen = false;
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = speed;
        }

        private void Start()
        {
            SetState(State.Chase);
            enemyState.Enemy = this;

            //Make drop rates easily transitable later
            float totalRate = 0;
            foreach (DropRateObject drop in droppableLoot)
                totalRate += drop.rate;

            droppableLoot[0] = new DropRateObject(droppableLoot[0].gameObject, droppableLoot[0].rate / totalRate);
            for (int i = 1; i < droppableLoot.Count; ++i)
                droppableLoot[i] = new DropRateObject(droppableLoot[i].gameObject, (droppableLoot[i - 1].rate + droppableLoot[i].rate) / totalRate);
        }

        private void Update()
        {
            enemyState.Behaviour();

            if (health <= 0)
            {
                DropLoot();
                AudioManager.Instance.PlaySFX("EnemyDeath");
                Destroy(gameObject);
            }
        }

        private void DropLoot()
        {
            int itemsDropped = UnityEngine.Random.Range(minItemsDropped, maxItemsDropped + 1);
            for (int i = 0; i < itemsDropped; ++i)
                DropLootItem();
        }

        private void DropLootItem()
        {
            Vector2 dropSpot = dropRadius * UnityEngine.Random.insideUnitCircle;
            float lootDropped = UnityEngine.Random.value;
            foreach (DropRateObject drop in droppableLoot)
                if (drop.rate > lootDropped)
                {
                    GameObject loot = Instantiate(drop.gameObject, this.transform.position, Quaternion.identity);
                    TweenMovement lootMovement = loot.GetComponent<TweenMovement>();
                    lootMovement.xAxis.startValue = this.transform.position.x;
                    lootMovement.xAxis.endValue = this.transform.position.x + dropSpot.x;
                    lootMovement.yAxis.startValue = this.transform.position.y;
                    lootMovement.yAxis.endValue = this.transform.position.y + dropHeight;
                    lootMovement.zAxis.startValue = this.transform.position.z;
                    lootMovement.zAxis.endValue = this.transform.position.z + dropSpot.y;
                    return;
                }
        }

        public void SetState(State newState)
        {
            switch (newState)
            {
                case State.Chase:
                    enemyState = new Chase();
                    break;
                case State.Attack:
                    enemyState = new Attack();
                    break;
                case State.Return:
                    enemyState = new Return();
                    break;
                default:
                    break;
            }
            enemyState.Enemy = this;
            enemyState.BB = this.bb;
        }

        public void UpdateLife()
        {

            ui_health.gameObject.SetActive(true);
            ui_health.GetComponentInChildren<Image>().fillAmount = (this as IDamageable).HealthRatio;
        }


        public void Slow()
        {

            agent.speed = slowSpeed;
        }

        public void UnSlow()
        {

            agent.speed = speed;
        }

    }
}