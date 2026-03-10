using UnityEngine;

namespace Combat
{
    public interface IAttacker
    {

        /// <summary>
        /// Real damage dealt after calculations, if applicable
        /// </summary>
        public int Damage { get; }

        public void Attack(GameObject target)
        {
            if (!target.TryGetComponent<IDamageable>(out IDamageable hp)) return;
            hp.Damage(Damage);
        }
    }
}