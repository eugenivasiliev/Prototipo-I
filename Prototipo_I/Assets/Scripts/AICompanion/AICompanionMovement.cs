using UnityEngine;
using Utils;

namespace AICompanion
{
    public class AICompanionMovement : TweenMovement
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform target;
        [SerializeField, Range(0, 5)] private float arrivalDistance;
        [SerializeField, Range(0, 5)] private float chaseCooldown;
        private bool hasArrived => Vector3.Distance(this.transform.position, target.position) < arrivalDistance;
        private float yPosition;
        private float currentChaseCooldown = 0;

        override protected void Start()
        {
            xAxis.startValue = this.transform.position.x;
            yPosition = this.transform.position.y;
            zAxis.startValue = this.transform.position.z;
        }

        void Update()
        {
            xAxis.SetActive(!hasArrived);
            yAxis.SetActive(!hasArrived);
            zAxis.SetActive(!hasArrived);

            xAxis.endValue = target.position.x;
            zAxis.endValue = target.position.z;

            if (hasArrived)
            {
                xAxis.Reset();
                yAxis.Reset();
                zAxis.Reset();

                xAxis.startValue = this.transform.position.x;
                zAxis.startValue = this.transform.position.z;

                currentChaseCooldown = 0;

                animator.SetBool("Is_Running", false);

                return;
            }

            currentChaseCooldown += Time.deltaTime;
            if (currentChaseCooldown < chaseCooldown) return;

            animator.SetBool("Is_Running", true);

            TweenUtil.Update(Time.deltaTime, ref xAxis);
            TweenUtil.Update(Time.deltaTime, ref yAxis);
            TweenUtil.Update(Time.deltaTime, ref zAxis);

            this.transform.position =
                Vector3.right * xAxis.value +
                Vector3.up * (yPosition + yAxis.value) +
                Vector3.forward * zAxis.value;

            Vector3 fwd = new Vector3(target.position.x - this.transform.position.x, 0, target.position.z - this.transform.position.z);

            this.transform.LookAt(this.transform.position + fwd);
        }
    }
}