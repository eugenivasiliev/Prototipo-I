using Trading;
using UnityEngine;

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
