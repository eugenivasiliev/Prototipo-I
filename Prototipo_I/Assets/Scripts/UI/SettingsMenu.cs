using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Toggle vSyncToggle;
        void Start()
        {
            fullscreenToggle.isOn = Screen.fullScreen;
            vSyncToggle.isOn = (QualitySettings.vSyncCount > 0);

            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
            vSyncToggle.onValueChanged.AddListener(SetVSync);
        }

        void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        void SetVSync(bool isVSyncOn)
        {
            QualitySettings.vSyncCount = isVSyncOn ? 1 : 0;
        }
    }
}