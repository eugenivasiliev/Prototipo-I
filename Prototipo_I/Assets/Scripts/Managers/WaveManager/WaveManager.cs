using System;
using System.Collections.Generic;
using UnityEngine;

using Wave = System.Collections.Generic.List<EnemyAI>;

[CreateAssetMenu(fileName = "WaveManager", menuName = "Scriptable Objects/Waves/Manager")]
public class WaveManager : ScriptableObject
{

    [Serializable]
    public struct PhaseEnemies
    {
        public string Name;
        public int biome;
        public int phase;
        public List<EnemyAI> enemies;
    }

    public readonly int difficultyTolerance = 3;
    public List<PhaseEnemies> waves { get; private set; }
    public Wave nextWave;

    public void ReadyNextWave(int biome, int phase, int enemyNumber, int difficulty)
    {
        PhaseEnemies phaseEnemies = GetPhaseEnemies(biome, phase);

        Wave wave = new Wave();
        do
        {
            for(int i = 0; i < enemyNumber; ++i)
            {
                wave.Add(phaseEnemies.enemies[UnityEngine.Random.Range(0, phaseEnemies.enemies.Count)]);
            }
        } while (Mathf.Abs(TotalDifficulty(wave) - difficulty) > difficultyTolerance);

        nextWave = wave;
    }

    private int TotalDifficulty(Wave wave)
    {
        int sum = 0;
        foreach(EnemyAI enemy in wave)
        {
            sum += enemy.Difficulty;
        }
        return sum;
    }

    private PhaseEnemies GetPhaseEnemies(int biome, int phase)
    {
        foreach(PhaseEnemies phaseEnemies in waves)
        {
            if(phaseEnemies.biome == biome && phaseEnemies.phase == phase) return phaseEnemies;
        }
        throw new MissingFieldException("Missing phase enemies!");
    }
}
