using UnityEngine;

namespace Utils
{
    public class Billboard : MonoBehaviour
    {
        private Camera mainCamera;
        [SerializeField] private bool fixedSize;
        [SerializeField] private float cameraBaseDistance;

        void Start()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            Follow();

            if (!fixedSize) return;

            this.GetComponent<RectTransform>().localScale = 
                Vector3.one * Vector3.Distance(this.transform.position, mainCamera.transform.position) / cameraBaseDistance;
        }

        void Follow() => 
            this.transform.rotation = mainCamera.transform.rotation;
    }
}