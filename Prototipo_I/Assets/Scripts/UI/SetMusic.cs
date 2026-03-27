using Audio;
using UnityEngine;

namespace UI
{
    public class SetMusic : MonoBehaviour
    {
        void Start()
        {
            AudioManager.Instance.PlayMusic("GameSceneDay");
        }

    }
}