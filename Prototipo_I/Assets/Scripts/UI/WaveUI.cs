using Enemies;
using System.Collections.Generic;
using Trading;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace UI
{
    public class WaveUI : MonoBehaviour
    {
        public void Start()
        {
            this.gameObject.SetActive(false);
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                WaveManager.Instance.ToggleWaveUI();
            }
        }

        public void ClickWave()
        {
            WaveManager.Instance.ToggleWaveUI();
            WaveManager.Instance.StartWave();
        }

    }
}