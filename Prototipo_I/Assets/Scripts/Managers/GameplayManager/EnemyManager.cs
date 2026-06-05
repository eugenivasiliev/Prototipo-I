using System.Collections;
using System.Collections.Generic;
using Farm;
using Objectives;
using UI;
using UI.Minimap;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private Minimap minimap;

        [SerializeField] private int currentBiomeIndex = 0;
        [SerializeField] private int currentPhaseIndex = 0;
        public int CurrentPhaseIndex { get { return currentPhaseIndex; } }
        public int TotalWaves;

        [SerializeField] private bool isWaveActive = false;
        public bool IsWaveActive { get { return isWaveActive; } }

        [SerializeField] private float timeToSpawn;
        [SerializeField] public List<SpawnZone> spawnZones = new List<SpawnZone>();

        private List<EnemyAI> allEnemies = new List<EnemyAI>();
        private List<GameObject> enemiesToSpawn = new List<GameObject>();

        [SerializeField] private GameObject plotManager;
        private List<Plot> allPlots = new List<Plot>();

        private System.Action<float> Spawn;
        private UnityEvent<float> Return = new UnityEvent<float>();

        [SerializeField] private EnemyAI.Blackboard bb;

        void Start()
        {
            allPlots.Clear();
            allPlots.AddRange(plotManager.GetComponentsInChildren<Plot>());
            this.bb.plots = allPlots;

            Spawn += SpawnEnemies;
            Return.AddListener((float t) => { ReturnToSpawn(); });

            DayNightCycle.Instance.SubscribeTimedEvent(Spawn, 1);
        }

        private bool AreEnemiesRemaining()
        {
            foreach (var enemy in allEnemies)
                if (enemy != null) return true;
            foreach (var zone in spawnZones)
                if (zone.EnemiesPendingCount() > 0) return true;
            return false;
        }

        private void Update()
        {
            if (!isWaveActive || AreEnemiesRemaining()) return;

            isWaveActive = false;
            currentPhaseIndex++;
            currentPhaseIndex = (int)Mathf.Min(currentPhaseIndex, TotalWaves - 1);

            if (ObjectivesManager.Instance.TryGetObjective<WavesCompleted, int>(out List<WavesCompleted> objs))
                foreach (var obj in objs)
                    obj.UpdateObjective(1);

            minimap.ClearSpawnZones();

            DayNightCycle.Instance.PassTime();
            DayNightCycle.Instance.SubscribeTimedEvent(Spawn, 1);
        }

        public void RegisterEnemy(EnemyAI enemy)
        {
            if (!allEnemies.Contains(enemy))
            {
                allEnemies.Add(enemy);
                EnemyAI.Blackboard enemyBB = this.bb;
                enemyBB.spawnZones = this.spawnZones;
                enemyBB.target = enemy.BB.target;
                enemyBB.attackRange = enemy.BB.attackRange;
                enemyBB.attackCooldown = enemy.BB.attackCooldown;
                enemy.BB = enemyBB;

                minimap?.AddEnemy(enemy.gameObject);
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
        }

        public void ReturnToSpawn()
        {
            foreach (var enemy in allEnemies)
                if (enemy != null) enemy.SetState(EnemyAI.State.Return);
            isWaveActive = false;
            DayNightCycle.Instance.PassTime();
            DayNightCycle.Instance.SubscribeTimedEvent(Spawn, 1);
        }
    }
}