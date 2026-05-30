using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Utils;

public class EnemyCounterList : MonoBehaviour
{
    [Serializable]
    public struct EnemyCounterInfo
    {
        public GameObject prefab;
        public int amount;
        public string name;
        public Sprite sprite;
    }

    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private List<EnemyCounterInfo> enemyCounterList;
    [SerializeField] private GameObject enemyCounterPrefab;
    [SerializeField] private Dictionary<GameObject, GameObject> enemyCounterInstances = new Dictionary<GameObject, GameObject>();
    [SerializeField, Range(0, 10)] private float listPadding;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadEnemies(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadEnemies(float t)
    {
        foreach (var enemy in enemyCounterInstances)
            Destroy(enemy.Value);
        enemyCounterInstances.Clear();

        foreach(SpawnZone zone in enemyManager.spawnZones)
        {
            foreach(GameObject enemy in zone.EnemiesPendingList())
            {
                if(enemyCounterInstances.ContainsKey(enemy))
                {
                    EnemyCounterInfo info = enemyCounterInstances[enemy].GetComponent<EnemyCounter>().Info;
                    info.amount++;
                    enemyCounterInstances[enemy].GetComponent<EnemyCounter>().Info = info;
                } else
                {
                    GameObject instance = Instantiate(enemyCounterPrefab, 
                        this.transform.position + enemyCounterInstances.Count * Vector3.down * listPadding, Quaternion.identity, transform);
                    foreach(EnemyCounterInfo info in enemyCounterList) 
                        if(info.prefab == enemy)
                            instance.GetComponent<EnemyCounter>().Info = info;
                    enemyCounterInstances.Add(enemy, instance);
                }
            }
        }

        DayNightCycle.Instance.SubscribeTimedEvent(LoadEnemies, 2);
    }
}
