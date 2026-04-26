using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace UI
{
    public class WaveStartedUI : SlidingPanelUI
    {
        [SerializeField, Range(0, 10)] private float imageHideDelay;
        private float imageHideCurrentDelay;
        protected override void Start() { }

        public void Play()
        {
            Toggle();
            imageHideCurrentDelay = imageHideDelay;
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