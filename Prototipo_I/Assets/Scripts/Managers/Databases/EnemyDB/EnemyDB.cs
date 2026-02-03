using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDB", menuName = "Scriptable Objects/Databases/EnemyDB")]
public class EnemyDB : ScriptableObject
{
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
    private Dictionary<string, GameObject> enemyPrefabsByName = new Dictionary<string, GameObject>();

    public void Init()
    {
        foreach(var enemy in enemyPrefabs)
        { 
            string name = enemy.name;
            Debug.Log(name);
            //Debug.Assert(!enemyPrefabsByName.ContainsKey(name));
            enemyPrefabsByName.Add(name, enemy);
        }
    }

    public GameObject GetEnemyFromName(string name)
    {
        GameObject obj = null;
        //Debug.Assert(enemyPrefabsByName.TryGetValue(name, out obj));
        enemyPrefabsByName.TryGetValue(name, out obj);
        return obj;
    }

    public EnemyAI GetAIFromName(string name)
    {
        GameObject obj = null;
        //Debug.Assert(enemyPrefabsByName.TryGetValue(name, out obj));
        enemyPrefabsByName.TryGetValue(name, out obj);
        return obj.GetComponent<EnemyAI>();
    }
}
