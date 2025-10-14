using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemieAI : MonoBehaviour
{
    public enum EnemyState
    {
        Chase,
        Attack,
        Return
    }

    [SerializeField] EnemyState _currentState = EnemyState.Chase;

    public EnemyState CurrentState { get => _currentState; private set => _currentState = value; }
    private NavMeshAgent agent;

    private Plot currentTargetPlot;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        switch(_currentState)
        {
            case EnemyState.Chase:
                //Debug.Log("Planta encontrada");
                Chase();
                break;
            case EnemyState.Attack:
                //Debug.Log("Atacando Plantas");
                Attack();
                break;
            case EnemyState.Return:
                //Debug.Log(hasReturn);
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
        if (DayNightCycle.Instance.DayTime <= 0f)
        {
            _currentState = EnemyState.Return;
        }

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
       if(DayNightCycle.Instance.DayTime <= 0f) 
       { 
          _currentState = EnemyState.Return; 
       }
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
        }
    }
}
