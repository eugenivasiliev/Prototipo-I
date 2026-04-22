using System.Collections;
using UnityEngine;
using Utils;

namespace GameMode
{
    public class WinConditionUI : MonoBehaviour
    {
        [SerializeField] private Tween<float> popupTween;
        [SerializeField] private string nextScene;
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
            SceneManager.LoadScene(nextScene);
        }
    }
}
