using UnityEngine;
using Utils;

public class DissolvedMaterial : MonoBehaviour
{
    private Material material;
    [SerializeField] private Tween<float> dissolveTween;
    [SerializeField] private bool destroyOnFinish;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        dissolveTween.value = dissolveTween.startValue;
    }

    public void Dissolve()
    {
        dissolveTween.SetActive(true);
    }

    private void Update()
    {
        TweenUtil.Update(Time.deltaTime, ref dissolveTween);
        material.SetFloat("_Dissolve", dissolveTween.value);

        if(destroyOnFinish && dissolveTween.value == dissolveTween.endValue) Destroy(gameObject);
    }
}
