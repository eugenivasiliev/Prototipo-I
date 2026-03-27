using UnityEngine;
using UnityEngine.InputSystem;
using UI;
public class InputActivator : MonoBehaviour
{
    [SerializeField] private WaveUI waveUI;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            InputSystem.actions.FindAction("wave_menu").started += ctx => { waveUI.ToggleWaveUI(); };
            InputSystem.actions.FindAction("close_menu").started += ctx => { if (waveUI.IsOpen()) waveUI.ToggleWaveUI(); };
        }
    }
}
