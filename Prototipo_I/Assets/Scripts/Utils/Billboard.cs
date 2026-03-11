using UnityEngine;

namespace Utils
{
    public class Billboard : MonoBehaviour
    {
        private Camera mainCamera;

        void Start()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            this.transform.rotation = mainCamera.transform.rotation;
        }
    }
}