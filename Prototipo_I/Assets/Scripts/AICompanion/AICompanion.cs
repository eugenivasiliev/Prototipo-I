using UnityEngine;

public class AICompanion : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private AnimationCurve verticalMovement;
    [SerializeField, Range(0, 5)] private float radius;
    [SerializeField] private float circlingSpeed;
    [SerializeField] private float soaringSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = playerTransform.position +
            Vector3.forward * radius * Mathf.Sin(Time.time * circlingSpeed) +
            Vector3.up * verticalMovement.Evaluate(Time.time * soaringSpeed) +
            Vector3.right * radius * Mathf.Cos(Time.time * circlingSpeed);

        this.transform.LookAt(this.transform.position + playerTransform.forward);
    }
}
