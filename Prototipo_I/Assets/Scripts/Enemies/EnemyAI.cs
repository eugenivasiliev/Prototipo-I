using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
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

    [SerializeField] private int damage;
    public int Damage { get => damage; }

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
