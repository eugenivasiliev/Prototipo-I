using System;
using System.Collections.Generic;
using UnityEngine;

public class FirePlant : PlantWeapon
{
    [SerializeField] private int damageDealt;
    [SerializeField] private float damageTime;
    private List<(IDamageable, float)> damageables = new List<(IDamageable, float)>();

    public override string Name => nameof(FirePlant);

    protected override void Update()
    {
        for (int i = 0; i < damageables.Count; ++i)
        {
            damageables[i] = (damageables[i].Item1, damageables[i].Item2 - Time.deltaTime);
            if(damageables[i].Item2 < 0)
            {
                damageables[i].Item1.Damage(damageDealt);
                damageables[i] = (damageables[i].Item1, damageTime);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.TryGetComponent(out IDamageable damageable)) damageables.Add((damageable, damageTime));
    }

    private void OnTriggerExit(Collider collider)
    {
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
