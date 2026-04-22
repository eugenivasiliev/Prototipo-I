using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class SpawnZone : MonoBehaviour
    {
        [SerializeField] private GameObject activeZoneIndicatorInstance;

        [SerializeField] private List<int> validPhases = new List<int>();
        public List<int> ValidPhases { get { return validPhases; } }

        public void ShowIndicator(int phaseIndex) =>
            activeZoneIndicatorInstance.SetActive(validPhases.Contains(phaseIndex));

        public void HideIndicator() =>
            activeZoneIndicatorInstance.SetActive(false);

        private void Update()
        {
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