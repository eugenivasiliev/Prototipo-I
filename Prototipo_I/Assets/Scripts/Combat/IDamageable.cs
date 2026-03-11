using UnityEngine;

namespace Combat
{
    public interface IDamageable
    {
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public float HealthRatio => (float)Health / (float)MaxHealth;

        public void Heal(int amount) => Health = Mathf.Min(Health + amount, MaxHealth);
        public void HealPercent(float percent) => Heal((int)(MaxHealth * percent / 100.0f));
        public void HealMax() => Health = MaxHealth;

        public void Damage(int amount) => Health = Mathf.Max(Health - amount, 0);
        public void DamagePercent(float percent) => Damage((int)(MaxHealth * percent / 100.0f));
        public void DamageMax() => Health = 0;

        public bool IsDead() => Health <= 0;
    }
}