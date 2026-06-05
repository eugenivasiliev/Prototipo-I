using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class FadingUI : MonoBehaviour
    {
        [SerializeField] private Tween<float> tween;
        [SerializeField] private Image image;

        // Update is called once per frame
        void Update()
        {
            TweenUtil.Update(Time.deltaTime, ref tween);
            image.color = new Color(image.color.r, image.color.g, image.color.b, tween.value);
        }
    }
}