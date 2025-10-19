using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager Instance {get; private set;}

    [SerializeField] private GameObject enemy;
    [SerializeField] private short enemiesByZone;
    [SerializeField] private short timeToSpawn;
    [SerializeField] public List<Transform> spawnZones = new List<Transform>();

    private List<EnemyAI> allEnemies = new List<EnemyAI>();

    private UnityEvent<float> Spawn = new UnityEvent<float>();
    private UnityEvent<float> Return = new UnityEvent<float>();
    void Start()
    {
        Instance = this;

        Spawn.AddListener(SpawnEnemies);
        Return.AddListener(ReturnToSpawn);

        DayNightCycle.Instance.SubscribeTimedEvent(Spawn, (DayNightCycle.Instance.DayCount + 0.5f) * DayNightCycle.Instance.DayDuration);
    }

    private void RegisterEnemy(EnemyAI enemy)
    {
        if (!allEnemies.Contains(enemy))
            allEnemies.Add(enemy);
    }

    private void SpawnEnemies(float t)
    {
        if (enemy == null) return;

        if (allEnemies.Count > 0)
        {
            foreach (var enemy in allEnemies)
            {
                if (enemy == null) continue;
                Destroy(enemy.gameObject);
            }
        }

        allEnemies.Clear();

        foreach (Transform zone in spawnZones)
            StartCoroutine(SpawnEnemyDelay(zone));

        DayNightCycle.Instance.SubscribeTimedEvent(Return, (DayNightCycle.Instance.DayCount + 1) * DayNightCycle.Instance.DayDuration);
    }

    private IEnumerator SpawnEnemyDelay (Transform zone)
    {
        for (int i = 0; i < enemiesByZone; i++)
        {
            GameObject enemyObject = Instantiate(enemy, zone.position, Quaternion.identity, zone.transform);
            EnemyAI enemyAI = enemyObject.GetComponent<EnemyAI>();

            if (enemyAI != null) RegisterEnemy(enemyAI);

            yield return new WaitForSeconds(timeToSpawn);
        }
    }

    private void ReturnToSpawn(float t)
    {
        foreach (var enemy in allEnemies)
            if (enemy != null) enemy.SetState(EnemyAI.State.Return);

        DayNightCycle.Instance.SubscribeTimedEvent(Spawn, (DayNightCycle.Instance.DayCount + 0.5f) * DayNightCycle.Instance.DayDuration);
    }
}
