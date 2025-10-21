using UnityEngine;

public class FirePlant : PlantWeapon
{
    [SerializeField] private int damageDealt;
    protected override void Update()
    {
        
    }

    private void OnTriggerStay(Collider collider)
    {
        if(collider.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(damageDealt);
        }
    }
}
