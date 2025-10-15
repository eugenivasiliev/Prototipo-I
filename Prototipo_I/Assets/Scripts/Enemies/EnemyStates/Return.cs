using System;
using UnityEngine;

public class Return : EnemyState
{
    private Transform targetSpawn = null;
    public override void Behaviour()
    {
        if (EnemieManager.Instance == null) return;

        if(targetSpawn != null)
        {
            float distance = Vector3.Distance(enemy.transform.position, targetSpawn.position);
            if (distance < 1.5f)
                GameObject.Destroy(enemy.gameObject);
            return;
        }

        float minDistance = Mathf.Infinity;

        foreach (var spawn in EnemieManager.Instance.spawnZones)
        {
            float distance = Vector3.Distance(enemy.transform.position, spawn.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                targetSpawn = spawn;
            }
        }

        if (targetSpawn != null)
            enemy.Agent.SetDestination(targetSpawn.position);
    }
}
