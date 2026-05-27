using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace VFX
{
    public class InstantaneousVFX : MonoBehaviour
    {
        [SerializeField] private float time;
        [SerializeField] private VisualEffect effect;
        void Start()
        {
            Debug.Log("Flag");
            effect.Play();
            StartCoroutine(SelfDestruct());
        }

        private IEnumerator SelfDestruct()
        {

            yield return new WaitForSeconds(time);

            Debug.Log("Flag");
            Destroy(gameObject);
        }
    }
}