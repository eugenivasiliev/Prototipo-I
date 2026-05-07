using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Minimap
{
    public class ScalingWaningIcon : MonoBehaviour
    {
        public Tween<float> waningTween;
        public Tween<Vector3> scalingTween;
        public Image image;
        public RectTransform icon;

        void Update()
        {
            TweenUtil.Update(Time.deltaTime, ref waningTween);
            image.color = new Color(image.color.r, image.color.g, image.color.b, waningTween.value);

            TweenUtil.Update(Time.deltaTime, ref scalingTween);
            icon.localScale = scalingTween.value;
        }
    }
}