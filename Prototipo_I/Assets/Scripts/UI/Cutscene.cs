using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace UI
{
    public class Cutscene : MonoBehaviour
    {
        [SerializeField] private VideoClip clip;
        [SerializeField, Range(0, 10)] private float playbackSpeed;
        [SerializeField] private VideoPlayer player;
        [SerializeField] private UnityEvent afterCutsceneEvent;

        void Start()
        {
            player.clip = clip;
            player.Play();
            player.loopPointReached += (VideoPlayer vp) => { afterCutsceneEvent.Invoke(); };
        }

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape)) {

                player.time += 10;
            }
        }
    }
}