using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace UI
{
    public class WaveStartedUI : SlidingPanelUI
    {
        [SerializeField] private VideoClip clip;
        [SerializeField, Range(0, 10)] private float playbackSpeed;
        [SerializeField] private VideoPlayer player;
        [SerializeField] private UnityEvent afterCutsceneEvent;
        protected override void Start() { }

        public void Play()
        {
            player.clip = clip;
            player.Play();
            player.loopPointReached += (VideoPlayer vp) => { afterCutsceneEvent.Invoke(); };
            Toggle();
        }
    }
}