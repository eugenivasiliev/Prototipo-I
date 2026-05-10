using System.Collections.Generic;
using Audio;
using UnityEngine;
using Utils;

namespace UI
{
    public class SetMusic : MonoBehaviour
    {
        [SerializeField] private List<string> phaseMusics = new List<string> { "GameSceneDay", "GameSceneNight" };
        private int curPhaseMusic = 0;

        void Start()
        {
            AudioManager.Instance.PlayMusicEvent(phaseMusics[curPhaseMusic]);
            DayNightCycle.Instance.SubscribeTimedEvent(ChangePhaseMusic, 1);
        }

        private void ChangePhaseMusic(float t)
        {
            curPhaseMusic++;
            curPhaseMusic %= phaseMusics.Count;
            AudioManager.Instance.PlayMusicEvent(phaseMusics[curPhaseMusic]);
            DayNightCycle.Instance.SubscribeTimedEvent(ChangePhaseMusic, 1);
        }

    }
}