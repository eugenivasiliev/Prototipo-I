using Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class Vines : MonoBehaviour
    {
        private int state = 0;
        [SerializeField, Range(0, 10)] private float[] cooldowns;
        private float curCooldown = 0;

        private List<EnemyAI> enemies = new List<EnemyAI>();

        [SerializeField] private GameObject particle;
        private Dictionary<EnemyAI, GameObject> allParticles = new Dictionary<EnemyAI, GameObject>();

        [SerializeField] private GameObject activeDecal;

        private void Start()
        {
            activeDecal.SetActive(false);
        }

        private void Update()
        {
            curCooldown += Time.deltaTime;
            if(curCooldown > cooldowns[state])
            {
                state = 1 - state;
                curCooldown = 0;
                activeDecal.SetActive(state == 1);
                if (state == 1) foreach (EnemyAI e in enemies) SlowDown(e);
                else foreach (EnemyAI e in enemies) UnSlowDown(e);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<EnemyAI>(out EnemyAI enemy)) return;

            enemies.Add(enemy);
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<EnemyAI>(out EnemyAI enemy)) return;

            enemies.Remove(enemy);

            if (state == 1) UnSlowDown(enemy);
        }

        private void SlowDown(EnemyAI enemy)
        {
            enemy.SlowDown();

            GameObject p = Instantiate(particle, enemy.transform.position, Quaternion.identity, enemy.transform);
            allParticles.Add(enemy, p);
        }

        private void UnSlowDown(EnemyAI enemy)
        {
            enemy.UnSlowDown();

            if (!allParticles.ContainsKey(enemy)) return;
            GameObject p = allParticles[enemy];
            Destroy(p);
            allParticles.Remove(enemy);
        }
    }
}