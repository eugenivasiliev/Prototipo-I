using UnityEngine;

namespace Utils
{
    public class Billboard : MonoBehaviour
    {
        private Vector3 startPos;
        private Vector3 startSize;
        float distance = 5.0f;
        private Camera mainCamera;

        void Start()
        {
            mainCamera = Camera.main;
            startPos = transform.position;
            startSize = transform.localScale;
        }

        void Update()
        {
            this.transform.rotation = mainCamera.transform.rotation;
            if (mainCamera.transform.position.y + distance < startPos.y)
            {
                Shrink();
            }
            else
            {
                UnShrink();
            }
        }

        private void Shrink()
        {
            this.transform.position = new Vector3(startPos.x, startPos.y - distance, startPos.z);
            this.transform.localScale = new Vector3(startSize.x - 0.5f, startSize.y - 0.5f, startSize.z);
        }
        private void UnShrink()
        {
            this.transform.position = new Vector3(startPos.x, startPos.y, startPos.z);
            this.transform.localScale = new Vector3(startSize.x, startSize.y, startSize.z);
        }
    }
}