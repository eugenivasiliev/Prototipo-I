using UnityEngine;

public class FirePuddlePlant : PlantWeapon
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private int damageDealt;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Rigidbody rb)) rb.maxLinearVelocity = maxSpeed;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable damageable)) damageable.Damage(damageDealt);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rb)) rb.maxLinearVelocity = float.MaxValue;
    }
}
