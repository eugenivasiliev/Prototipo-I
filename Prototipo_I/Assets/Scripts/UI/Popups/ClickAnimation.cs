using UnityEngine;

namespace UI
{
    public class ClickAnimation : MonoBehaviour
    {

        [SerializeField] Animator anim;

        void Start()
        {
        
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {

                transform.position = Input.mousePosition;
                anim.Play("clic", -1, 0f);
            }
        }
    }
}
