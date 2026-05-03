using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Enemies
{
    public class SpawnZone : MonoBehaviour
    {
        [SerializeField] private GameObject activeZoneIndicatorInstance;
        [SerializeField] private Tween<float> activeZoneIndicatorTween;
        [SerializeField, Range(0, 3)] private float activeZoneIndicatorSlowBlinkDuration;
        [SerializeField, Range(0, 3)] private float activeZoneIndicatorFastBlinkDuration;
        [SerializeField] private GameObject zoneArrowIndicatorInstance;

        [SerializeField] private List<int> validPhases = new List<int>();
        public List<int> ValidPhases { get { return validPhases; } }

        public void ShowIndicator(int phaseIndex)
        {
            bool shouldShow = validPhases.Contains(phaseIndex);
            activeZoneIndicatorInstance?.SetActive(shouldShow);
            activeZoneIndicatorTween.duration = activeZoneIndicatorSlowBlinkDuration;
            zoneArrowIndicatorInstance?.SetActive(shouldShow);
        }

        public void WaveStarted()
        {
            activeZoneIndicatorTween.duration = activeZoneIndicatorFastBlinkDuration;
            zoneArrowIndicatorInstance?.SetActive(false);
        }

        private void Update()
        {
            TweenUtil.Update(Time.deltaTime, ref activeZoneIndicatorTween);

            bool isBehindCamera = Vector3.Dot(this.transform.position - Camera.main.transform.position, Camera.main.transform.forward) < 0;

            activeZoneIndicatorInstance.GetComponent<Image>().color = new Color(1, 1, 1, (isBehindCamera) ? 0 : activeZoneIndicatorTween.value);

            Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
            Vector3 clampedScreenPos = new Vector3(
                Mathf.Clamp(screenPos.x, 0, Screen.width),
                Mathf.Clamp(screenPos.y, 0, Screen.height),
                screenPos.z
                );
            activeZoneIndicatorInstance.transform.localScale = (clampedScreenPos == screenPos) ? Vector3.one : 0.5f * Vector3.one;
            activeZoneIndicatorInstance.GetComponent<RectTransform>().position = clampedScreenPos;
        }
    }
}