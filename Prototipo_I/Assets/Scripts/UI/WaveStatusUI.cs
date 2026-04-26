using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using Utils;

namespace UI
{
    public class WaveStatusUI : SlidingPanelUI
    {
        [SerializeField, Range(0, 10)] private float imageHideDelay;
        private float imageHideCurrentDelay;
        [SerializeField] private int startShowPhase;
        [SerializeField] private int showPhaseSeparation = 2;
        protected override void Start() {
            DayNightCycle.Instance.SubscribeTimedEvent(Play, startShowPhase);
        }

        public void Play(float t)
        {
            Toggle();
            imageHideCurrentDelay = imageHideDelay;
            DayNightCycle.Instance.SubscribeTimedEvent(Play, showPhaseSeparation);
        }

        protected override void Update()
        {
            base.Update();

            if (isHidden) return;

            imageHideCurrentDelay -= Time.deltaTime;
            if (imageHideCurrentDelay <= 0)
                Toggle();
        }
    }
}