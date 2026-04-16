using Enemies;
using Player;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using static Enemies.WaveDB;

namespace UI
{
    public class WaveUI : MonoBehaviour, IContexted
    {
        [SerializeField] private EnemyManager enemyManager;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Canvas ui;

        private bool isOpen = false;
        public bool IsOpen() { return isOpen; }
        private void Start()
        {
            if (SceneManager.GetSceneName() != "Tutorial")
            {
                InputSystem.actions.FindAction("wave_menu").started += ctx => { ToggleWaveUI(); };
                InputSystem.actions.FindAction("close_menu").started += ctx => { if (isOpen) ToggleWaveUI(); };
            }
        }

        public void ClickWave()
        {
            ToggleWaveUI();
            DayNightCycle.Instance.PassTime();
        }

        public void ToggleWaveUI()
        {
            if (enemyManager.IsWaveActive) return;

            isOpen = !isOpen;
            Cursor.lockState = (isOpen) ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isOpen;
            ui.gameObject.SetActive(isOpen);
            Time.timeScale = (isOpen) ? 0f : 1f;
            playerController.MovementLocked = isOpen;
        }

        public bool ContextKeyActive() => !enemyManager.IsWaveActive;
    }
}