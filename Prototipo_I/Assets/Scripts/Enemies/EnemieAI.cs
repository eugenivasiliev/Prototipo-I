using System;
using UnityEngine;
using UnityEngine.AI;
public class EnemieAI : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Chase,
        Attack
    }

    [SerializeField] EnemyState _currentState = EnemyState.Idle;
    private NavMeshAgent agent;

    private Transform player;

    private float minDistance;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        switch(_currentState)
        {
            case EnemyState.Idle:
                Debug.Log("Buscando Plants");
                break;
            case EnemyState.Chase:
               // Debug.Log("Planta encontrada");
                Chase();
                break;
            case EnemyState.Attack:
                Debug.Log("Atacando Plantas");
                Attack();
                break;
            default:
                break;
        }
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
            agent.SetDestination(minPlot.transform.position);

        }
    }

    private void Attack()
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentState = EnemyState.Chase;
    }
}
