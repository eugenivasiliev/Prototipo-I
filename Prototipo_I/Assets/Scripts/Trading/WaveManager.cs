using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    [SerializeField] private GameObject waveUI;

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
        waveUI.SetActive(!waveUI.activeSelf);
        Cursor.visible = waveUI.activeSelf;
        Cursor.lockState = (waveUI.activeSelf) ? CursorLockMode.None : CursorLockMode.Locked;
        PlayerController.MovementLocked = waveUI.activeSelf;
    }

    public void StartWave() {

        DayNightCycle.Instance.PassTime();
    }
}
