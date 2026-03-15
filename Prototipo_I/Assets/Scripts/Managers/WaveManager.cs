using System.Collections.Generic;
using Player;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Utils;

namespace Enemies
{
    public class WaveManager : Singleton<WaveManager>, IInteractable
    {
        [SerializeField] private GameObject waveUI;
        [SerializeField] private PlayerController playerController;
        private bool isOpen = false;

        public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding>{
            new IInteractable.KeyBinding("wave_console", InputActionChange.ActionCanceled, Action_OpenMenu)
        };

        private void Start()
        {
            InitSingleton();
        }

        public void ToggleWaveUI()
        {
            isOpen = !isOpen;
            Cursor.lockState = (isOpen) ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isOpen;
            waveUI.SetActive(isOpen);
            Time.timeScale = (isOpen) ? 0f : 1f;
            playerController.MovementLocked = isOpen;
        }

        public void StartWave()
        { 
            DayNightCycle.Instance.PassTime();
        }

        private void Action_OpenMenu(InputAction.CallbackContext context)
        {
            ToggleWaveUI();
        }

        public void OnInteract()
        {
            throw new System.NotImplementedException();
        }
    }
}