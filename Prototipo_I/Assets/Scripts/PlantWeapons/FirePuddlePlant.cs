using System.Collections.Generic;
using UnityEngine;

public class FirePuddlePlant : PlantWeapon
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private int damageDealt;
    [SerializeField] private float damageTime;
    private List<(IDamageable, float)> damageables;

    protected override void Update()
    {
        for (int i = 0; i < damageables.Count; ++i)
        {
            damageables[i] = (damageables[i].Item1, damageables[i].Item2 - Time.deltaTime);
            if (damageables[i].Item2 < 0)
            {
                damageables[i].Item1.Damage(damageDealt);
                damageables[i] = (damageables[i].Item1, damageTime);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out IDamageable damageable)) damageables.Add((damageable, damageTime));
        if (collider.gameObject.TryGetComponent(out Rigidbody rb)) rb.maxLinearVelocity = maxSpeed;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Rigidbody rb)) rb.maxLinearVelocity = float.MaxValue;
        if (!collider.TryGetComponent(out IDamageable damageable)) return;
        for (int i = 0; i < damageables.Count; ++i)
        {
            if (damageables[i].Item1 == damageable)
            {
                damageables.RemoveAt(i);
                return;
            }
        }
    }
}
