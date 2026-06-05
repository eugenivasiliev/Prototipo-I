using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Combat;
using Farm;
using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Formats.Alembic.Importer;
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
            public List<SpawnZone> spawnZones;
            public Transform barricadeTransform;
            public float attackCooldown;
            public float curAttackCooldown;
            public float attackRange;
        }

        public enum Target
        {
            Home,
            Plots,
            Player,
            Barricade
        }

        public enum State
        {
            Chase,
            Attack,
            Return
        }

        [Header("Audio")]
        [SerializeField] protected string walkSound;
        public string WalkSound { get => walkSound; }
        [SerializeField] protected string attackSound;
        public string AttackSound { get => attackSound; }

        [SerializeField] protected EnemyState enemyState;

        protected NavMeshAgent agent;
        public NavMeshAgent Agent { get => agent; }

        [SerializeField] protected int health;

        public int Health { get => health; set => health = value; }
        protected int maxHealth;
        public int MaxHealth { get => maxHealth; set { } }
        [SerializeField] protected Canvas ui_health;
        private Image ui_health_image;
        private float barSpeed = 1f;

        [SerializeField] protected int damage;
        public int Damage => damage;

        [SerializeField] protected int difficulty;


        [SerializeField] protected float speed;
        [SerializeField] protected float slowSpeed;
        private bool aboutToDie = false;
        [SerializeField] private Renderer matHolder;
        [SerializeField] private Material blinkMat;
        [SerializeField] private Material originalMat;
        [SerializeField] private float blinkTime;
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
        [SerializeField] protected List<DropRateObject> droppableLoot;
        [SerializeField] protected int minItemsDropped;
        [SerializeField] protected int maxItemsDropped;
        [SerializeField, Range(0, 5)] protected float dropRadius;
        [SerializeField, Range(0, 5)] protected float dropHeight = 2;

        [SerializeField] protected Blackboard bb;
        public Blackboard BB { get => bb; set => bb = value; }

        public enum AnimationType
        {
            ALEMBIC,
            FBX
        }

        [Header("Animation")]
        [SerializeField] protected AnimationType animationType;
        [SerializeField] protected AlembicStreamPlayer alembicStreamPlayer;
        [SerializeField] protected Animator animator;
        public Animator Animator { get => animator; }
        [SerializeField] protected bool isDying = false;
        [SerializeField] protected AnimationClip deathAnim;
        [SerializeField] protected GameObject deathPrefab;
        [SerializeField] protected GameObject damageParticle;


        bool frozen = false;
        protected void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = speed * 2;
            maxHealth = health;
        }


        protected void Start()
        {
            SetState(State.Chase);
            enemyState.Enemy = this;

            
            float totalRate = 0;
            foreach (DropRateObject drop in droppableLoot)
                totalRate += drop.rate;

            droppableLoot[0] = new DropRateObject(droppableLoot[0].gameObject, droppableLoot[0].rate / totalRate);
            for (int i = 1; i < droppableLoot.Count; ++i)
                droppableLoot[i] = new DropRateObject(droppableLoot[i].gameObject, (droppableLoot[i - 1].rate + droppableLoot[i].rate) / totalRate);

            ui_health_image = ui_health.gameObject.transform.GetChild(1).GetComponent<Image>();
        }

        protected virtual void Update()
        {
            if (isDying) return;

            if (health <= 0)
            {
                isDying = true;

                Instantiate(deathPrefab, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
                return;
            }

            enemyState.Behaviour();

            if (ui_health_image.fillAmount > (this as IDamageable).HealthRatio)
                ui_health_image.fillAmount = Mathf.MoveTowards(ui_health_image.fillAmount, (this as IDamageable).HealthRatio, barSpeed * Time.deltaTime);
        }

        
        protected IEnumerator AlembicDeathAnim()
        {
            while (alembicStreamPlayer.CurrentTime <= alembicStreamPlayer.EndTime - 0.05f)
            {
                alembicStreamPlayer.CurrentTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            DropLoot();
            Destroy(gameObject);
        }

        protected IEnumerator FBXDeathAnim()
        {
            animator.SetBool("IsDying", true);
            yield return new WaitForSeconds(deathAnim.length);
            

            DropLoot();
            Destroy(gameObject);
        }

        protected void DropLoot()
        {
            int itemsDropped = UnityEngine.Random.Range(minItemsDropped, maxItemsDropped + 1);
            for (int i = 0; i < itemsDropped; ++i)
                DropLootItem();
        }

        protected void DropLootItem()
        {
            Vector2 dropSpot = dropRadius * UnityEngine.Random.insideUnitCircle;
            float lootDropped = UnityEngine.Random.value;
            foreach (DropRateObject drop in droppableLoot)
                if (drop.rate > lootDropped)
                {
                    GameObject loot = Instantiate(drop.gameObject, this.transform.position, Quaternion.identity);
                    TweenMovement lootMovement = loot.GetComponent<TweenMovement>();
                    /*lootMovement.xAxis.startValue = this.transform.position.x;
                    lootMovement.xAxis.endValue = this.transform.position.x + dropSpot.x;
                    lootMovement.yAxis.startValue = this.transform.position.y;
                    lootMovement.yAxis.endValue = this.transform.position.y + dropHeight;
                    lootMovement.zAxis.startValue = this.transform.position.z;
                    lootMovement.zAxis.endValue = this.transform.position.z + dropSpot.y;*/
                    return;
                }
        }

        public void SetState(State newState)
        {
            if(enemyState != null) enemyState.OnExit();

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

            enemyState.OnEnter();
        }

        public void UpdateLife()
        {
            ui_health.gameObject.SetActive(true);
            


            StartCoroutine(TurnRed());
        }

        public void OnDamage()
        {
            UpdateLife();
            Instantiate(damageParticle, this.transform.position, Quaternion.identity);
        }

        protected void SetSpeed(int i) {
            agent.speed = i * 2;
        }


        public void SlowDown() {
            SetSpeed((int)slowSpeed);
        }

        public void UnSlowDown() {
            SetSpeed((int)speed);
        }
        public void MightDie(int damage)
        {
            aboutToDie = health - damage > 0;
        }

        public bool IsAboutToDie()
        {
            return aboutToDie;
        }

        public void GetBarricade(Transform t)
        {
            bb.barricadeTransform = t;
            bb.targetTransform = bb.barricadeTransform;
            bb.target = Target.Barricade;
        }


        IEnumerator TurnRed()
        {
            if (!matHolder) yield break;
            matHolder.material = blinkMat;
            yield return new WaitForSeconds(blinkTime);
            matHolder.material = originalMat;
        }

    }



    }