using System;
using UnityEngine;
using UnityEngine.AI;
public class EnemieAI : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Return
    }

    [SerializeField] EnemyState _currentState = EnemyState.Idle;

    public EnemyState CurrentState { get => _currentState; private set => _currentState = value; }
    private NavMeshAgent agent;

    private Plot currentTargetPlot;

    private bool hasReturn = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        switch(_currentState)
        {
            case EnemyState.Idle:
                Debug.Log("Buscando Plants");
                _currentState = EnemyState.Chase;
                break;
            case EnemyState.Chase:
               // Debug.Log("Planta encontrada");
                Chase();
                break;
            case EnemyState.Attack:
                Debug.Log("Atacando Plantas");
                Attack();
                break;
            case EnemyState.Return:
                if(!hasReturn)
                    ReturnToSpawn();
                break;
            default:
                break;
        }
    }

    public void SetState(EnemyState newState)
    {
        _currentState = newState;
    }
    private void Chase()
    {
        if (PlotManager.Instance == null) return;

        Plot minPlot = null;
        float minDistance = Mathf.Infinity;

        foreach (var plot in PlotManager.Instance.plots)
        {
            if(!plot.IsPlanted) continue;

            float distance = Vector3.Distance(transform.position, plot.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                minPlot = plot;
            }
        }

        if (minPlot != null)
        {
            if (currentTargetPlot != minPlot)
            {
                currentTargetPlot = minPlot;
                agent.SetDestination(minPlot.transform.position);
            }

            float distToTarget = Vector3.Distance(transform.position, currentTargetPlot.transform.position);
            if (distToTarget < 1.5f)
            {
                _currentState = EnemyState.Attack;
            }
        }
    }

    private void Attack()
    {

    }

    private void ReturnToSpawn()
    {
        if (EnemieManager.Instance == null) return;

        Transform minSpawn = null;
        float minDistance = Mathf.Infinity;

        foreach (var spawn in EnemieManager.Instance.spawnZones)
        {
            float distance = Vector3.Distance(transform.position, spawn.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                minSpawn = spawn;
            }
        }

        if (minSpawn != null)
        {
            agent.SetDestination(minSpawn.transform.position);
            hasReturn = true;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
}
