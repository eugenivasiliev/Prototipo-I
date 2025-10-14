using UnityEngine;

public class FirePlant : PlantWeapon
{
    [SerializeField] private int damageDealt;
    protected override void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(damageDealt);
        }
    }
}
