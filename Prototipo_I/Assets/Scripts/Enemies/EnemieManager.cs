using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemieManager : MonoBehaviour
{

    public static EnemieManager Instance {get; private set;}

    [SerializeField] private GameObject enemie;
    [SerializeField] private short enemiesByZone;
    [SerializeField] public List<Transform> spawnZones = new List<Transform>();

    private List<EnemieAI> allEnemies = new List<EnemieAI>();

    private float halfDayTime;
    private float endDayTime;

    private UnityEvent<float> Spawn = new UnityEvent<float>();
    private UnityEvent<float> Return = new UnityEvent<float>();
    void Start()
    {
        Instance = this;

        halfDayTime = DayNightCycle.Instance.DayDuration / 2;
        endDayTime = DayNightCycle.Instance.DayDuration;
        //Debug.Log(halfDayTime);
        Spawn.AddListener(SpawnEnemies);
        Return.AddListener(ReturnToSpawn);

        DayNightCycle.Instance.SubscribeTimedEvent(Spawn, DayNightCycle.Instance.DayDuration - halfDayTime);
        DayNightCycle.Instance.SubscribeTimedEvent(Return, DayNightCycle.Instance.DayDuration - endDayTime);
    }

    private void RegisterEnemy(EnemieAI enemy)
    {
        if (!allEnemies.Contains(enemy))
            allEnemies.Add(enemy);
    }

    private void SpawnEnemies(float usseless)
    {
        if (enemie == null) { return; }

        foreach (Transform zone in spawnZones)
        {
            StartCoroutine(SpawnEnemyDelay(zone));
        }
    }

    private IEnumerator SpawnEnemyDelay (Transform zone)
    {
               for (int i = 0; i < enemiesByZone; i++)
                {
                    GameObject enemyObj = Instantiate(enemie, zone.position, zone.rotation);
                    EnemieAI enemyai = enemyObj.GetComponent<EnemieAI>();
                    if (enemyai != null)
                    {
                        RegisterEnemy(enemyai);
                    }
                    yield return new WaitForSeconds(1);
                }
    }

    private void ReturnToSpawn()
    {
        foreach (var enemy in allEnemies)
        {
            if (enemy != null)
            {
                enemy.SetState(EnemieAI.EnemyState.Return);
            }
        }
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
