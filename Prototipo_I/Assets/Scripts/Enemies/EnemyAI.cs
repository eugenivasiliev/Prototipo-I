using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour, IAttacker, IDamageable
{
    public enum Difficult : int
    {
        Easy = 1,
        Medium = 2,
        Hard = 3
    }

    public enum State
    {
        Chase,
        Attack,
        Return
    }

    [SerializeField] private EnemyState enemyState = new Chase();

    private NavMeshAgent agent;
    public NavMeshAgent Agent { get => agent; }

    [SerializeField] private int health;

    public int Health { get => health; set => health = value; }
    public int MaxHealth { get => 100; set { } }

    [SerializeField] private int damage;
    public int Damage => damage;

    [SerializeField] private int difficulty;
    public int Difficulty => difficulty;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        enemyState.Enemy = this;
    }

    private void Update()
    {
        enemyState.Behaviour();

        if(health <= 0 )
            Destroy( gameObject );
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
    }
}
