using UnityEngine;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    [SerializeField] private GameObject pauseMenuPanel;

    private bool isPaused = false;

    public bool IsPaused => isPaused;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        pauseMenuPanel.SetActive(false);
    }

    private void Update()
    {
        // Toggle pause con la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Resume llamado desde objeto: " + gameObject.name
            + " en escena: " + gameObject.scene.name);
        Debug.Log("Panel activo? " + (pauseMenuPanel != null && pauseMenuPanel.activeSelf));
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
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


}
