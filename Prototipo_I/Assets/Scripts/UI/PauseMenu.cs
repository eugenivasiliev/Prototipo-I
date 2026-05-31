using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class PauseMenu : Singleton<PauseMenu>, IInteractable
    {
        [SerializeField] private GameObject pauseMenuPanel;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private List<GameObject> subPanels;

        private bool isPaused = false;

        public bool IsPaused => isPaused;

        public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding>
    {
        new IInteractable.KeyBinding("pause", InputActionChange.ActionCanceled, ToggleMenu)
    };

        private void Awake()
        {
            InitSingleton();

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
            playerController.MovementLocked = isPaused;
            if (!isPaused) foreach(GameObject panel in subPanels) panel.SetActive(false);
        }

        public void Resume()
        {
            isPaused = true;
            ToggleMenu(new InputAction.CallbackContext());
        }

        public void QuitGame()
        {
            Time.timeScale = 1f;

            

        Application.Quit();

        }

        public void OnInteract()
        {
            throw new System.NotImplementedException();
        }
    }
}