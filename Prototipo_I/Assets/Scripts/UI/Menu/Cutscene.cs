using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using Utils;
namespace UI
{
    public class Cutscene : MonoBehaviour
    {
        [SerializeField] private VideoClip clip;
        [SerializeField, Range(0, 10)] private float playbackSpeed;
        [SerializeField] private VideoPlayer player;
        //[SerializeField] private UnityEvent afterCutsceneEvent;

        [SerializeField] private GameObject nextCutscene;

        void Start()
        {
            player.clip = clip;
            player.Play();
            //player.loopPointReached += (VideoPlayer vp) => { afterCutsceneEvent.Invoke(); };
        }

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) 
                || Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.JoystickButton7)) {

                if (nextCutscene != null)
                {
                    nextCutscene.SetActive(true);
                    gameObject.SetActive(false);
                }
                else
                    SceneManager.LoadScene("Tutorial");

            }
        }
    }
}