using UnityEngine;

public class MaterialInstanceDissolveModifier : MonoBehaviour
{
    private Material material;
    [SerializeField] public float dissolveValue;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    private void Update()
    {
        material.SetFloat("_Dissolve", dissolveValue);
    }
}
