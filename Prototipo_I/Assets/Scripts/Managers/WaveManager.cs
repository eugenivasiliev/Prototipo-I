using Player;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Utils;

namespace Enemies
{
    public class WaveManager : MonoBehaviour
    {
        public static WaveManager Instance;
        [SerializeField] private GameObject waveUI;

        private bool isOpen = false;

        private void Start()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void ToggleWaveUI()
        {
            isOpen = !isOpen;
            Cursor.lockState = (isOpen) ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isOpen;
            waveUI.SetActive(isOpen);
            Time.timeScale = (isOpen) ? 0f : 1f;
            PlayerController.MovementLocked = isOpen;
        }

        public void StartWave()
        {

            DayNightCycle.Instance.PassTime();
        }
    }
}