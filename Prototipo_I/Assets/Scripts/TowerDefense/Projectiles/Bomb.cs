using UnityEngine;

namespace TowerDefense
{
    public class Bomb : Projectile
    {

        [SerializeField] private GameObject explosion;

        protected override void HitTarget()
        {
            Instantiate(explosion, this.transform.position, this.transform.rotation);
            Destroy(gameObject);
        }
    }
}