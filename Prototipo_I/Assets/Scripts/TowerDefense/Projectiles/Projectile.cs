using Combat;
using UnityEngine;
using Utils;

namespace TowerDefense
{
    public class Projectile : TweenMovement
    {
        public Vector3 startPos;
        public GameObject target;
        protected IDamageable damageable;

        [SerializeField, Range(0, 100)] protected int damage;
        [SerializeField, Range(0, 1)] protected float hitTolerance = 0.05f;

        protected override void Start()
        {
            if(!target.TryGetComponent<IDamageable>(out damageable))
            {
                Destroy(this.gameObject);
                return;
            }

            xAxis.startValue = startPos.x;
            yAxis.startValue = startPos.y;
            zAxis.startValue = startPos.z;

            xAxis.endValue = target.transform.position.x;
            yAxis.endValue = target.transform.position.y;
            zAxis.endValue = target.transform.position.z;

            xAxis.SetActive(true);
            yAxis.SetActive(true);
            zAxis.SetActive(true);
        }

        void Update()
        {
            if(target == null)
            {
                Destroy(this.gameObject);
                return;
            }

            xAxis.endValue = target.transform.position.x;
            yAxis.endValue = target.transform.position.y;
            zAxis.endValue = target.transform.position.z;

            TweenUtil.Update(Time.deltaTime, ref xAxis);
            TweenUtil.Update(Time.deltaTime, ref yAxis);
            TweenUtil.Update(Time.deltaTime, ref zAxis);

            this.transform.position = new Vector3(xAxis.value, yAxis.value, zAxis.value);

            if (xAxis.t >= xAxis.duration - hitTolerance)
                HitTarget();
        }

        protected virtual void HitTarget()
        {
            damageable.Damage(damage);
            Destroy(this.gameObject);
        }
    }
}