using UnityEngine;

public class PuddlePlant : PlantWeapon
{
    [SerializeField] private float maxSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Rigidbody rb)) rb.maxLinearVelocity = maxSpeed;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rb)) rb.maxLinearVelocity = float.MaxValue;
    }
}
