using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager Instance {get; private set;}

    [SerializeField] private int currentBiomeIndex = 0;
    [SerializeField] private int currentPhaseIndex = 0;

    [SerializeField] private bool isWaveActive = false;

    [SerializeField] private short timeToSpawn;
    [SerializeField] public List<Transform> spawnZones = new List<Transform>();

    private List<EnemyAI> allEnemies = new List<EnemyAI>();
    private List<string> enemiesToSpawn = new List<string>();

    private UnityEvent<float> Spawn = new UnityEvent<float>();
    private UnityEvent<float> Return = new UnityEvent<float>();
    void Start()
    {
        Instance = this;

        Spawn.AddListener(SpawnEnemies);
        Return.AddListener(ReturnToSpawn);

        SpawnEnemies(.5f);

        DayNightCycle.Instance.SubscribeTimedEvent(Spawn, (DayNightCycle.Instance.DayCount + 0.5f) * DayNightCycle.Instance.DayDuration);
    }

    private void RegisterEnemy(EnemyAI enemy)
    {
        if (!allEnemies.Contains(enemy))
            allEnemies.Add(enemy);
    }

    private void SpawnEnemies(float t)
    {
        isWaveActive = true;

        Debug.Log("Spawning");

        if (allEnemies.Count > 0)
        {
            foreach (var enemy in allEnemies)
            {
                if (enemy == null) continue;
                Destroy(enemy.gameObject);
            }
        }

        allEnemies.Clear();

        WaveDBManager.Instance.DB.ReadyNextWave(currentBiomeIndex, currentPhaseIndex);
        enemiesToSpawn = WaveDBManager.Instance.DB.nextWave;

        foreach (Transform zone in spawnZones)
            StartCoroutine(SpawnEnemyDelay(zone));

        DayNightCycle.Instance.SubscribeTimedEvent(Return, (DayNightCycle.Instance.DayCount + 1) * DayNightCycle.Instance.DayDuration);
    }

    private IEnumerator SpawnEnemyDelay (Transform zone)
    {


        while(enemiesToSpawn.Count > 0)
        {
            Debug.Log(enemiesToSpawn.Count);

            string name = enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)];
            GameObject prefab = EnemyDBManager.Instance.DB.GetEnemyFromName(name);
            GameObject enemyObject = Instantiate(prefab, zone.position, Quaternion.identity, zone.transform);
            EnemyAI enemyAI = enemyObject.GetComponent<EnemyAI>();

            if (enemyAI != null) RegisterEnemy(enemyAI);

            enemiesToSpawn.Remove(name);

            yield return new WaitForSeconds(timeToSpawn);
        }

        if(isWaveActive)
        {
            isWaveActive = false;
            currentPhaseIndex++;
            currentPhaseIndex = (int)Mathf.Min(currentPhaseIndex, 1);
            if(ObjectivesManager.Instance.TryGetObjective<WavesCompleted, int>(out List<WavesCompleted> objs))
            {
                foreach (var obj in objs)
                {
                    obj.UpdateObjective(1);
                }
            }
        }
    }

    private void ReturnToSpawn(float t)
    {
        foreach (var enemy in allEnemies)
            if (enemy != null) enemy.SetState(EnemyAI.State.Return);

        DayNightCycle.Instance.SubscribeTimedEvent(Spawn, (DayNightCycle.Instance.DayCount + 0.5f) * DayNightCycle.Instance.DayDuration);
    }
}
