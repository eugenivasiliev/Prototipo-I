using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 finalPos;

    public GameObject target;

    float time = 0.0f;
    private float maxTime;
    void Start()
    {
        startPos = transform.position;
        maxTime = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (target != null) finalPos = target.transform.position;

        this.transform.position = (time/maxTime) * finalPos + (1- (time / maxTime)) * startPos;

        if (time > maxTime) { 

            if (target != null)
                DamageTarget();            

            Destroy(gameObject);
        }
    }

    void DamageTarget()
    {
        if (target == null) return;

        if (target.TryGetComponent<IDamageable>(out var damageable))
            damageable.DamageMax();
    }
}
