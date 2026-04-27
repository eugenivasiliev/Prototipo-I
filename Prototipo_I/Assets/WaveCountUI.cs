using System.Collections.Generic;
using Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Utils;

namespace UI
{
    public class WaveCountUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private EnemyManager enemyManager;
        [SerializeField] private LocalizedString localizedString;

        void Start()
        {
            Dictionary<string, string> values = new Dictionary<string, string> {
            { "curWave", enemyManager.CurrentPhaseIndex.ToString() },
            { "totalWaves", enemyManager.TotalWaves.ToString() }
        };
            localizedString.Arguments = new object[] { values };
            text.text = localizedString.GetLocalizedString();
            DayNightCycle.Instance.SubscribeTimedEvent(RenderText, 1);
        }

        private void RenderText(float t)
        {
            Dictionary<string, string> values = new Dictionary<string, string> {
            { "curWave", enemyManager.CurrentPhaseIndex.ToString() },
            { "totalWaves", enemyManager.TotalWaves.ToString() }
        };
            localizedString.Arguments = new object[] { values };
            text.text = localizedString.GetLocalizedString();
            DayNightCycle.Instance.SubscribeTimedEvent(RenderText, 2);
        }
    }
}