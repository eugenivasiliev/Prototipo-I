using UnityEngine;

namespace Utils
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;
        [SerializeField] private bool fixedSize;
        [SerializeField] private float cameraBaseDistance;

        void Start()
        {
            if(targetCamera == null)
                targetCamera = Camera.main;
        }

        void Update()
        {
            Follow();

            if (!fixedSize) return;

            this.GetComponent<RectTransform>().localScale = 
                Vector3.one * Vector3.Distance(this.transform.position, targetCamera.transform.position) / cameraBaseDistance;
        }

        void Follow() => 
            this.transform.rotation = targetCamera.transform.rotation;
    }
}