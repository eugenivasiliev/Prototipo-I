using UnityEngine;
using Utils;

namespace GameMode
{
    public class LossConditionUI : MonoBehaviour
    {
        [SerializeField] private Tween<float> popupTween;

        void Start()
        {
            popupTween.SetActive(true);
        }

        void Update()
        {
            if (TweenUtil.Update(Time.deltaTime, ref popupTween))
                this.transform.localScale = popupTween.value * Vector3.one;
            else
                SceneManager.LoadScene("MainMenu");
        }
    }
}