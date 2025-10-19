using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private SceneManager instance;
    public SceneManager Instance { get { return instance; } }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void LoadScene(string sceneName) =>
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

    public static void LoadScene(int buildIndex) =>
        UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);

    public static void Exit() =>
        UnityEngine.Application.Quit();
}
