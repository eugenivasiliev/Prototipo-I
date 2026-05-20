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

        [SerializeField] private List<Animator> activeAnimators;

        [SerializeField] private int minRandParticles = 3;
        [SerializeField] private int maxRandParticles = 10;
        private List<GameObject> randParticles = new List<GameObject>();

        [SerializeField, Range(0, 30)] private float radius = 20f;

        private bool IsActive => state == 1;

        private void Start()
        {
            activeDecal.SetActive(false);
            foreach(Animator anim in activeAnimators)
                anim.SetBool("IsActive", false);
        }

        private void Update()
        {
            curCooldown += Time.deltaTime;
            if(curCooldown > cooldowns[state])
            {
                state = 1 - state;
                curCooldown = 0;
                activeDecal.SetActive(IsActive);
                foreach (Animator anim in activeAnimators)
                    anim.SetBool("IsActive", IsActive);
                if (IsActive)
                {
                    foreach (EnemyAI e in enemies) SlowDown(e);
                    int randParticleCount = UnityEngine.Random.Range(minRandParticles, maxRandParticles + 1);
                    for (int i = 0; i < randParticleCount; i++)
                    {
                        Vector2 posXZ = UnityEngine.Random.insideUnitCircle * radius * this.transform.localScale;
                        Vector3 pos = new Vector3(posXZ.x, 0, posXZ.y);
                        Quaternion rot = UnityEngine.Random.rotation;
                        randParticles.Add(Instantiate(particle, this.transform.position + pos, rot, this.transform));
                    }
                }
                else
                {
                    foreach (EnemyAI e in enemies) UnSlowDown(e);
                    while (randParticles.Count > 0)
                    {
                        Destroy(randParticles[randParticles.Count - 1]);
                        randParticles.RemoveAt(randParticles.Count - 1);
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<EnemyAI>(out EnemyAI enemy)) return;

            enemies.Add(enemy);

            if (IsActive) SlowDown(enemy);
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<EnemyAI>(out EnemyAI enemy)) return;

            enemies.Remove(enemy);

            if (IsActive) UnSlowDown(enemy);
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