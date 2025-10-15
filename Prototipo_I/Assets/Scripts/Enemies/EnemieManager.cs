using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemieManager : MonoBehaviour
{

    public static EnemieManager Instance {get; private set;}

    [SerializeField] private GameObject enemie;
    [SerializeField] private short enemiesByZone;
    [SerializeField] private short timeToSpawn;
    [SerializeField] public List<Transform> spawnZones = new List<Transform>();

    private List<EnemieAI> allEnemies = new List<EnemieAI>();

    private float halfDayTime;
    private int lastDayCount = -1;
    private bool canSpawn = true;

    private UnityEvent<float> Spawn = new UnityEvent<float>();
    private UnityEvent<float> Return = new UnityEvent<float>();
    void Start()
    {
        Instance = this;

        halfDayTime = DayNightCycle.Instance.DayDuration / 2;

        Spawn.AddListener(SpawnEnemies);
        Return.AddListener(ReturnToSpawn);

        DayNightCycle.Instance.SubscribeTimedEvent(Spawn, (DayNightCycle.Instance.DayCount + 0.5f) * DayNightCycle.Instance.DayDuration);
    }

    private void RegisterEnemy(EnemieAI enemy)
    {
        if (!allEnemies.Contains(enemy))
            allEnemies.Add(enemy);
    }

    private void SpawnEnemies(float usseless)
    {
        if (enemie == null) { return; }

        //if(allEnemies.Count > 0)
        //{
        //    foreach (var enemy in allEnemies)
        //    {
        //        Destroy(enemy.gameObject);
        //    }
        //}

        //allEnemies.Clear();

        foreach (Transform zone in spawnZones)
        {
            StartCoroutine(SpawnEnemyDelay(zone));
        }

        DayNightCycle.Instance.SubscribeTimedEvent(Return, (DayNightCycle.Instance.DayCount + 1) * DayNightCycle.Instance.DayDuration);
    }

    private IEnumerator SpawnEnemyDelay (Transform zone)
    {
        for (int i = 0; i < enemiesByZone; i++)
        {
            GameObject enemyObject = Instantiate(enemie, zone.position, zone.rotation);
            EnemieAI enemyAI = enemyObject.GetComponent<EnemieAI>();

            if (enemyAI != null)
            {
                RegisterEnemy(enemyAI);
            }
            yield return new WaitForSeconds(timeToSpawn);
        }
    }

    private void ReturnToSpawn(float useless)
    {
        foreach (var enemy in allEnemies)
        {
            if (enemy != null)
            {
                enemy.SetState(EnemieAI.EnemyState.Return);
            }
        }

        DayNightCycle.Instance.SubscribeTimedEvent(Spawn, (DayNightCycle.Instance.DayCount + 0.5f) * DayNightCycle.Instance.DayDuration);
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = allEnemies.Count - 1; i >= 0; i--)
        {
            EnemieAI enemy = allEnemies[i];

            if (enemy == null) 
            {
                allEnemies.RemoveAt(i);
                continue;
            }

            if (enemy.CurrentState == EnemieAI.EnemyState.Return)
            {
                foreach (Transform spawn in spawnZones)
                {
                    if (Vector3.Distance(enemy.transform.position, spawn.position) < 1.5f)
                    {
                        Destroy(enemy.gameObject);
                        allEnemies.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}
