using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private EnemyDB enemyDB;
    [SerializeField] private WaveDB waveDB;

    [SerializeField] private int currentBiomeIndex = 0;
    [SerializeField] private int currentPhaseIndex = 0;

    [SerializeField] private bool isWaveActive = false;

    [SerializeField] private float timeToSpawn;
    [SerializeField] public List<SpawnZone> spawnZones = new List<SpawnZone>();

    private List<EnemyAI> allEnemies = new List<EnemyAI>();
    private List<string> enemiesToSpawn = new List<string>();

    [SerializeField] private GameObject plotManager;
    private List<Plot> allPlots = new List<Plot>();

    private UnityEvent<float> Spawn = new UnityEvent<float>();
    private UnityEvent<float> Return = new UnityEvent<float>();

    [SerializeField] private EnemyAI.Blackboard bb;
    void Start()
    {
        Init();
        enemyDB.Init();

        allPlots.Clear();
        allPlots.AddRange(plotManager.GetComponentsInChildren<Plot>());
        this.bb.plots = allPlots;

        Spawn.AddListener(SpawnEnemies);
        Return.AddListener(ReturnToSpawn);

        DayNightCycle.Instance.SubscribeTimedEvent(Spawn, 1);
    }

    private bool AreEnemiesRemaining()
    {
        foreach (var enemy in allEnemies)
            if (enemy != null) return true;
        return false;
    }

    private void Update()
    {
        if (!isWaveActive || AreEnemiesRemaining() || enemiesToSpawn.Count > 0) return;

        isWaveActive = false;
        currentPhaseIndex++;
        currentPhaseIndex = (int)Mathf.Min(currentPhaseIndex, waveDB.Waves.Count - 1);

        if (ObjectivesManager.Instance.TryGetObjective<WavesCompleted, int>(out List<WavesCompleted> objs))
            foreach (var obj in objs)
                obj.UpdateObjective(1);

        DayNightCycle.Instance.PassTime();
        DayNightCycle.Instance.SubscribeTimedEvent(Spawn, 1);
    }

    private void RegisterEnemy(EnemyAI enemy)
    {
        if (!allEnemies.Contains(enemy))
        {
            allEnemies.Add(enemy);
            EnemyAI.Blackboard enemyBB = this.bb;
            enemyBB.target = enemy.BB.target;
            enemy.BB = enemyBB;
        }
    }

    private void SpawnEnemies(float t)
    {
        isWaveActive = true;

        if (allEnemies.Count > 0)
        {
            foreach (var enemy in allEnemies)
            {
                if (enemy == null) continue;
                Destroy(enemy.gameObject);
            }
        }

        allEnemies.Clear();

        waveDB.ReadyNextWave(currentBiomeIndex, currentPhaseIndex, enemyDB);
        enemiesToSpawn = waveDB.nextWave;

        foreach (SpawnZone zone in spawnZones)
            if(zone.ValidPhases.Contains(currentPhaseIndex)) StartCoroutine(SpawnEnemyDelay(zone));
    }

    private IEnumerator SpawnEnemyDelay (SpawnZone zone)
    {


        while(enemiesToSpawn.Count > 0)
        {
            string name = enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)];
            GameObject prefab = enemyDB.GetEnemyFromName(name);
            GameObject enemyObject = Instantiate(prefab, zone.transform.position, Quaternion.identity, zone.transform);
            EnemyAI enemyAI = enemyObject.GetComponent<EnemyAI>();

            if (enemyAI != null) RegisterEnemy(enemyAI);

            enemiesToSpawn.Remove(name);

            yield return new WaitForSeconds(timeToSpawn);
        }
    }

    private void ReturnToSpawn(float t)
    {
        foreach (var enemy in allEnemies)
            if (enemy != null) enemy.SetState(EnemyAI.State.Return);

        DayNightCycle.Instance.PassTime();
        DayNightCycle.Instance.SubscribeTimedEvent(Spawn, 1);
    }
}
