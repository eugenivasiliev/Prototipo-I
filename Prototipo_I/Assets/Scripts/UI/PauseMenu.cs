using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour, IInteractable
{
    public static PauseMenu Instance { get; private set; }

    [SerializeField] private GameObject pauseMenuPanel;

    private bool isPaused = false;

    public bool IsPaused => isPaused;

    public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding>
    {
        new IInteractable.KeyBinding("pause", InputActionChange.ActionCanceled, ToggleMenu)
    };

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        pauseMenuPanel.SetActive(false);
    }

    private void Start()
    {
        (this as IInteractable).Bind();
    }

    private void ToggleMenu(InputAction.CallbackContext ctx)
    {
        isPaused = !isPaused;
        Cursor.lockState = (isPaused) ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
        pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = (isPaused) ? 0f : 1f;
        PlayerController.MovementLocked = isPaused;
    }

    public void Resume()
    {
        isPaused = true;
        ToggleMenu(new InputAction.CallbackContext());
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void OnInteract()
    {
        throw new System.NotImplementedException();
    }
}
