using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace UI
{
    public class HoverDilating : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Tween<float> tween;
        [SerializeField] private RectTransform rectTransform;
        private Vector3 baseScale;

        void Start()
        {
            baseScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tween.Reset();
            tween.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tween.Reverse();
            tween.SetActive(true);
        }

        void Update()
        {
            if (TweenUtil.Update(Time.deltaTime, ref tween))
                rectTransform.localScale = tween.value * baseScale;
        }
    }
}