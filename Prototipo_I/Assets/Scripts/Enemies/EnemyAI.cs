using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyAI : MonoBehaviour, IAttacker, IDamageable
{
    [Serializable]
    public struct Blackboard
    {
        public Target target;
        public Transform targetTransform;
        public Transform homeTransform;
        public PlotManager plotManager;
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

    [SerializeField] private Blackboard bb;
    public Blackboard BB { get => bb; set => bb = value; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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
            AudioManager.instance.PlaySFX("EnemyDeath");
            Destroy(gameObject);
        }
    }

    private void DropLoot()
    {
        int itemsDropped = UnityEngine.Random.Range(minItemsDropped, maxItemsDropped + 1);
        for(int i = 0; i < itemsDropped; ++i)
            DropLootItem();
    }

    private void DropLootItem()
    {
        Vector2 dropSpot = dropRadius * UnityEngine.Random.insideUnitCircle;
        float lootDropped = UnityEngine.Random.value;
        foreach (DropRateObject drop in droppableLoot)
            if (drop.rate > lootDropped)
            {
                Instantiate(
                    drop.gameObject, 
                    this.transform.position + Vector3.right * dropSpot.x + Vector3.forward * dropSpot.y, 
                    Quaternion.identity
                    );
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

    public void UpdateLife() {

        ui_health.gameObject.SetActive(true);
        ui_health.GetComponentInChildren<Image>().fillAmount = (this as IDamageable).HealthRatio;
    }

}
