using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "EnemyDB", menuName = "Scriptable Objects/Databases/EnemyDB")]
    public class EnemyDB : ScriptableObject
    {
        [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
        private Dictionary<string, GameObject> enemyPrefabsByName = new Dictionary<string, GameObject>();

        public void Init()
        {
            foreach (var enemy in enemyPrefabs)
            {
                if(!enemyPrefabsByName.ContainsKey(enemy.name))
                    enemyPrefabsByName.Add(enemy.name, enemy);
            }
        }

        public GameObject GetEnemyFromName(string name)
        {
            GameObject obj = null;
            enemyPrefabsByName.TryGetValue(name, out obj);
            return obj;
        }

        public EnemyAI GetAIFromName(string name)
        {
            GameObject obj = null;
            enemyPrefabsByName.TryGetValue(name, out obj);
            return obj.GetComponent<EnemyAI>();
        }
    }
}