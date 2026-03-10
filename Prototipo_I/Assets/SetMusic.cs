using Audio;
using UnityEngine;

namespace UI
{
    public class SetMusic : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            AudioManager.Instance.PlayMusic("GameSceneDay");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}