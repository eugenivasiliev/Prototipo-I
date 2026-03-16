using UnityEngine;

namespace Utils
{
    public class Billboard : MonoBehaviour
    {
        private Vector3 startPos;
        private Vector3 startSize;
        float distance = 5.0f;
        private Camera mainCamera;

        enum Billboards
        {
            BUTTON,
            HEALTH
        }

        [SerializeField] private Billboards billboards = Billboards.BUTTON;

        void Start()
        {
            mainCamera = Camera.main;
            startPos = transform.position;
            startSize = transform.localScale;
        }

        void Update()
        {
            switch (billboards)
            {
                case Billboards.HEALTH:
                    Follow();
                    break;

                case Billboards.BUTTON:
                    CheckHeight();
                    break;

                default:
                    break;
            }

            
        }

        void CheckHeight() {

            Follow();

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

        void Follow() {
            this.transform.rotation = mainCamera.transform.rotation;
        }
    }
}