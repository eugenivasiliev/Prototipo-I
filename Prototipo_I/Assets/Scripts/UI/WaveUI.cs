using Enemies;
using Trading;
using UnityEngine;

namespace UI
{
    public class WaveUI : MonoBehaviour
    {
        public void Start()
        {
            this.gameObject.SetActive(false);
        }

        public void ClickWave()
        {
            WaveManager.Instance.StartWave();
            WaveManager.Instance.ToggleWaveUI();
        }

    }
}