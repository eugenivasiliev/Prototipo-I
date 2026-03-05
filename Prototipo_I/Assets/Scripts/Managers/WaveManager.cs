using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

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
        /*
        waveUI.SetActive(!waveUI.activeSelf);
        Cursor.visible = waveUI.activeSelf;
        Cursor.lockState = (waveUI.activeSelf) ? CursorLockMode.None : CursorLockMode.Locked;
        PlayerController.MovementLocked = waveUI.activeSelf;
        */
        isOpen = !isOpen;
        Cursor.lockState = (isOpen) ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;
        waveUI.SetActive(isOpen);
        Time.timeScale = (isOpen) ? 0f : 1f;
        PlayerController.MovementLocked = isOpen;
    }

    public void StartWave() {

        DayNightCycle.Instance.PassTime();
    }
}
