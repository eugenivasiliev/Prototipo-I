using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "WaveDB", menuName = "Scriptable Objects/Databases/WaveDB")]
    public class WaveDB : ScriptableObject
    {

        [Serializable]
        public struct Wave
        {
            [SerializeField] public List<EnemyAI> list;
        }

        [Serializable]
        public struct PhaseEnemies
        {
            [SerializeField] public string Name;
            [SerializeField] public int biome;
            [SerializeField] public int phase;
            [SerializeField] public int difficulty;
            [SerializeField] public int enemyCount;
            [SerializeField] public List<GameObject> enemies;
        }

        public readonly int difficultyTolerance = 3;
        [SerializeField] private List<PhaseEnemies> waves;
        public List<PhaseEnemies> Waves => waves;
        public List<GameObject> nextWave;

        public void ReadyNextWave(int biome, int phase)
        {
            PhaseEnemies phaseEnemies = GetPhaseEnemies(biome, phase);

            List<GameObject> wave = new List<GameObject>();
            do
            {
                for (int i = 0; i < phaseEnemies.enemyCount; ++i)
                {
                    wave.Add(phaseEnemies.enemies[UnityEngine.Random.Range(0, phaseEnemies.enemies.Count)]);
                }
            } while (Mathf.Abs(TotalDifficulty(wave) - phaseEnemies.difficulty) > difficultyTolerance);

            nextWave = wave;
        }

        private int TotalDifficulty(List<GameObject> wave)
        {
            int sum = 0;
            foreach (GameObject enemy in wave)
            {
                sum += enemy.GetComponent<EnemyAI>().Difficulty;
            }
            return sum;
        }

        private PhaseEnemies GetPhaseEnemies(int biome, int phase)
        {
            foreach (PhaseEnemies phaseEnemies in waves)
            {
                if (phaseEnemies.biome == biome && phaseEnemies.phase == phase) return phaseEnemies;
            }
            throw new MissingFieldException("Missing phase enemies!");
        }
    }
}