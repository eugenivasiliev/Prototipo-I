using System;
using System.Collections;
using System.Collections.Generic;
using UI.Minimap;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Enemies
{
    public class SpawnZone : MonoBehaviour
    {
        [Serializable]
        public struct PhaseEnemies
        {
            [SerializeField] public int phase;
            [SerializeField] public List<GameObject> enemies;
            [SerializeField] public float spawnDelay;
        }

        [SerializeField] private Minimap minimap;
        [SerializeField] private EnemyManager enemyManager;
        [SerializeField] private GameObject activeZoneIndicatorInstance;
        [SerializeField] private Tween<float> activeZoneIndicatorTween;
        [SerializeField] private Tween<Vector3> scalingTween;
        [SerializeField, Range(0, 3)] private float activeZoneIndicatorSlowBlinkDuration;
        [SerializeField, Range(0, 3)] private float activeZoneIndicatorFastBlinkDuration;
        [SerializeField] private GameObject zoneArrowIndicatorInstance;

        [SerializeField] private List<PhaseEnemies> phaseInfo = new List<PhaseEnemies>();
        public List<PhaseEnemies> ValidPhases { get { return phaseInfo; } }

        private int currentPhaseIndex;

        private void Start()
        {
            WaveEnded(0);
        }

        private int FindValidPhase(int phaseIndex)
        {
            for (int i = 0; i < phaseInfo.Count; i++)
                if(phaseInfo[i].phase == phaseIndex)
                    return i;

            return -1;
        }

        private void Update()
        {
            TweenUtil.Update(Time.deltaTime, ref activeZoneIndicatorTween);
            bool isBehindCamera = Vector3.Dot(this.transform.position - Camera.main.transform.position, Camera.main.transform.forward) < 0;
            activeZoneIndicatorInstance.GetComponent<Image>().color = new Color(1, 1, 1, (isBehindCamera) ? 0 : activeZoneIndicatorTween.value);

            Vector2 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
            (Vector2 screenCenter, Vector2 clippedPos) = 
                Utils.Utils.clipSegmentToRectangle(Utils.Utils.screenSize / 2.0f, screenPos, Vector2.zero, Utils.Utils.screenSize);
            TweenUtil.Update(Time.deltaTime, ref scalingTween);
            activeZoneIndicatorInstance.GetComponent<RectTransform>().localScale = scalingTween.value * ((clippedPos == screenPos) ? 1.0f : 0.5f);
            activeZoneIndicatorInstance.GetComponent<RectTransform>().position = clippedPos;
        }

        private void WaveStarted(float t)
        {
            activeZoneIndicatorTween.duration = activeZoneIndicatorFastBlinkDuration;
            scalingTween.duration = activeZoneIndicatorFastBlinkDuration;
            zoneArrowIndicatorInstance?.SetActive(false);

            if (currentPhaseIndex != -1)
                StartCoroutine(SpawnEnemyDelay());

            DayNightCycle.Instance.SubscribeTimedEvent(WaveEnded, 1);
        }

        private void WaveEnded(float t)
        {
            currentPhaseIndex = FindValidPhase(enemyManager.CurrentPhaseIndex);

            bool shouldShow = currentPhaseIndex != -1;
            activeZoneIndicatorInstance?.SetActive(shouldShow);
            activeZoneIndicatorTween.duration = activeZoneIndicatorSlowBlinkDuration;
            scalingTween.duration = activeZoneIndicatorSlowBlinkDuration;
            zoneArrowIndicatorInstance?.SetActive(shouldShow);
            if (shouldShow) minimap.AddSpawnZone(this.gameObject);

            DayNightCycle.Instance.SubscribeTimedEvent(WaveStarted, 1);
        }

        private IEnumerator SpawnEnemyDelay()
        {
            while (phaseInfo[currentPhaseIndex].enemies.Count > 0)
            {
                int enemyIndex = UnityEngine.Random.Range(0, phaseInfo[currentPhaseIndex].enemies.Count);
                GameObject prefab = phaseInfo[currentPhaseIndex].enemies[enemyIndex];
                GameObject enemyInstance = Instantiate(prefab, this.transform.position, Quaternion.identity, this.transform);
                EnemyAI enemyAI = enemyInstance.GetComponent<EnemyAI>();

                if (enemyAI != null) enemyManager.RegisterEnemy(enemyAI);

                phaseInfo[currentPhaseIndex].enemies.RemoveAt(enemyIndex);

                yield return new WaitForSeconds(phaseInfo[currentPhaseIndex].spawnDelay);
            }
        }

        public int EnemiesPendingCount()
        {
            if(currentPhaseIndex == -1) return 0;
            return phaseInfo[currentPhaseIndex].enemies.Count;
        }

        public List<GameObject> EnemiesPendingList()
        {
            if (currentPhaseIndex == -1) return new List<GameObject>();
            return phaseInfo[currentPhaseIndex].enemies;
        }
    }
}