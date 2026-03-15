using System;
using UnityEngine;

namespace Enemies
{
    public class Return : EnemyState
    {
        private Transform targetSpawn = null;
        public override void Behaviour()
        {
            if (EnemyManager.Instance == null) return;

            if (targetSpawn != null)
            {
                float distance = Vector3.Distance(enemy.transform.position, targetSpawn.position);
                if (distance < 1.5f)
                    GameObject.Destroy(enemy.gameObject);
                return;
            }

            float minDistance = Mathf.Infinity;

            foreach (var spawn in EnemyManager.Instance.spawnZones)
            {
                float distance = Vector3.Distance(enemy.transform.position, spawn.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetSpawn = spawn.transform;
                }
            }

            if (targetSpawn != null)
                enemy.Agent.SetDestination(targetSpawn.position);
        }
    }
}