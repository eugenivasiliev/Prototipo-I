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
        [SerializeField] private Tween<Vector3> scalingTween;
        [SerializeField, Range(0, 3)] private float activeZoneIndicatorSlowBlinkDuration;
        [SerializeField, Range(0, 3)] private float activeZoneIndicatorFastBlinkDuration;
        [SerializeField] private GameObject zoneArrowIndicatorInstance;

        [SerializeField] private List<int> validPhases = new List<int>();
        public List<int> ValidPhases { get { return validPhases; } }

        public bool ShowIndicator(int phaseIndex)
        {
            bool shouldShow = validPhases.Contains(phaseIndex);
            activeZoneIndicatorInstance?.SetActive(shouldShow);
            activeZoneIndicatorTween.duration = activeZoneIndicatorSlowBlinkDuration;
            scalingTween.duration = activeZoneIndicatorSlowBlinkDuration;
            zoneArrowIndicatorInstance?.SetActive(shouldShow);
            return shouldShow;
        }

        public void WaveStarted()
        {
            activeZoneIndicatorTween.duration = activeZoneIndicatorFastBlinkDuration;
            scalingTween.duration = activeZoneIndicatorFastBlinkDuration;
            zoneArrowIndicatorInstance?.SetActive(false);
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
    }
}