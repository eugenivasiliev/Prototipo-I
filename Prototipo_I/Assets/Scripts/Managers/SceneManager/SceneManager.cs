using Audio;
using UnityEngine;

namespace Utils
{
    public class SceneManager : Singleton<SceneManager>
    {
        private void Awake()
        {
            InitSingleton();
        }

        public static void LoadScene(string sceneName)
        {
            AudioManager.Instance.StopMusic();
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }


        public static void LoadScene(int buildIndex) =>
            UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);

        public static void Exit() =>
            UnityEngine.Application.Quit();
    }
}