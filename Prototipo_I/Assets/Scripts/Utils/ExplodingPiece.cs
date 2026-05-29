using UnityEngine;

public class ExplodingPiece : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float minForce;
    [SerializeField, Range(0f, 100f)] private float maxForce;

    void Start() =>
        this.GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * Random.Range(minForce, maxForce), ForceMode.Impulse);
}
