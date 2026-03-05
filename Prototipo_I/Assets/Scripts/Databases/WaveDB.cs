using System;
using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField] public List<string> enemies;
    }

    public readonly int difficultyTolerance = 3;
    [SerializeField] private List<PhaseEnemies> waves;
    public List<PhaseEnemies> Waves => waves;
    public List<string> nextWave;

    public void ReadyNextWave(int biome, int phase, EnemyDB enemyDB)
    {
        PhaseEnemies phaseEnemies = GetPhaseEnemies(biome, phase);

        List<string> wave = new List<string>();
        do
        {
            for(int i = 0; i < phaseEnemies.enemyCount; ++i)
            {
                wave.Add(phaseEnemies.enemies[UnityEngine.Random.Range(0, phaseEnemies.enemies.Count)]);
            }
        } while (Mathf.Abs(TotalDifficulty(wave, enemyDB) - phaseEnemies.difficulty) > difficultyTolerance);

        nextWave = wave;
    }

    private int TotalDifficulty(List<string> wave, EnemyDB enemyDB)
    {
        int sum = 0;
        foreach(string enemy in wave)
        {
            sum += enemyDB.GetAIFromName(enemy).Difficulty;
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
