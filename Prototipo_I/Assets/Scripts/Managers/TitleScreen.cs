using Audio;
using UnityEngine;

namespace UI
{
    public class TitleScreen : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            AudioManager.Instance.PlayMusicEvent("TitleScene");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}