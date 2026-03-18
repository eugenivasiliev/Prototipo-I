using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace UI
{
    public class SlidingPanelUI : MonoBehaviour
    {
        [SerializeField] protected RectTransform panel;
        protected InputSystem_Actions inputs;

        [SerializeField] protected Vector4 hiddenPos;
        [SerializeField] protected Vector4 visiblePos;

        [SerializeField] protected Tween<float> tween;

        protected bool isHidden = false;
        void Start()
        {
            if (inputs == null) inputs = new InputSystem_Actions();
            inputs.Player.Enable();
            inputs.Player.objectives_toggle.performed += Toggle;
        }

        protected virtual void Update()
        {
            TweenUtil.Update(Time.deltaTime, ref tween);

            Vector2 anchorMin = new Vector2(
                (1 - tween.value) * hiddenPos.x + tween.value * visiblePos.x,
                (1 - tween.value) * hiddenPos.y + tween.value * visiblePos.y
                );

            Vector2 anchorMax = new Vector2(
                (1 - tween.value) * hiddenPos.z + tween.value * visiblePos.z,
                (1 - tween.value) * hiddenPos.w + tween.value * visiblePos.w
                );

            panel.anchorMin = anchorMin;
            panel.anchorMax = anchorMax;
        }

        void Toggle(InputAction.CallbackContext ctx)
        {
            if (!isHidden)
                tween.Reverse();
            else
            {

                tween.Reset();
                tween.SetActive(true);
            }

            isHidden = !isHidden;
        }
    }
}