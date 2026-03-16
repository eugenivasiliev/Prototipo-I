using UnityEngine;
using Utils;

namespace TowerDefense
{
    public class Projectile : TweenMovement
    {
        public Vector3 startPos;
        public Transform finalPos;

        [SerializeField, Range(0, 1)] private float hitTolerance = 0.05f;

        float time = 0.0f;
        public float maxTime;
        protected override void Start()
        {
            xAxis.startValue = startPos.x;
            yAxis.startValue = startPos.y;
            zAxis.startValue = startPos.z;

            xAxis.endValue = finalPos.position.x;
            yAxis.endValue = finalPos.position.y;
            zAxis.endValue = finalPos.position.z;

            xAxis.SetActive(true);
            yAxis.SetActive(true);
            zAxis.SetActive(true);
        }

        void Update()
        {

            xAxis.endValue = finalPos.position.x;
            yAxis.endValue = finalPos.position.y;
            zAxis.endValue = finalPos.position.z;

            TweenUtil.Update(Time.deltaTime, ref xAxis);
            TweenUtil.Update(Time.deltaTime, ref yAxis);
            TweenUtil.Update(Time.deltaTime, ref zAxis);

            this.transform.position = new Vector3(xAxis.value, yAxis.value, zAxis.value);

            if (xAxis.t >= xAxis.duration - hitTolerance) Destroy(this.gameObject);
        }
    }
}