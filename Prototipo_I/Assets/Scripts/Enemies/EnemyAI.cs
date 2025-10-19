using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour, IAttacker, IDamageable
{
    public enum State
    {
        Chase,
        Attack,
        Return
    }

    [SerializeField] private EnemyState enemyState = new Chase();

    private NavMeshAgent agent;
    public NavMeshAgent Agent { get => agent; }

    public int Health { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int MaxHealth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    [SerializeField] private int damage;
    public int Damage => damage;

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
