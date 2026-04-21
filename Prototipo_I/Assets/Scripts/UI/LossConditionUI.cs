using System.Collections;
using UnityEngine;
using Utils;

namespace GameMode
{
    public class LossConditionUI : MonoBehaviour
    {
        [SerializeField] private Tween<float> popupTween;
        private float waitTime = 2.5f;
        void Start()
        {
            popupTween.SetActive(true);
            StartCoroutine(Leave());
        }

        void Update()
        {
            if (TweenUtil.Update(Time.deltaTime, ref popupTween))
                this.transform.localScale = popupTween.value * Vector3.one;
        }

        public IEnumerator Leave()
        {
            yield return new WaitForSeconds(waitTime);
            SceneManager.LoadScene("MainMenu");
        }
    }
}