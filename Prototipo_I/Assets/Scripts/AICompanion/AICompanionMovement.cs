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
        private bool previousHasArrived;
        private float yPosition;
        private float currentChaseCooldown = 0;

        [Header("Leaning")]
        [SerializeField] private Tween<float> leanAngle;
        [SerializeField] private GameObject pivot;

        override protected void Start()
        {
            xAxis.startValue = this.transform.position.x;
            yPosition = this.transform.position.y;
            zAxis.startValue = this.transform.position.z;

            previousHasArrived = hasArrived;
        }

        void Update()
        {
            leanAngle.SetActive(true);

            xAxis.SetActive(!hasArrived);
            yAxis.SetActive(!hasArrived);
            zAxis.SetActive(!hasArrived);

            xAxis.endValue = target.position.x;
            zAxis.endValue = target.position.z;

            if (hasArrived)
            {
                leanAngle.Reverse();

                xAxis.Reset();
                yAxis.Reset();
                zAxis.Reset();

                xAxis.startValue = this.transform.position.x;
                zAxis.startValue = this.transform.position.z;

                currentChaseCooldown = 0;

                animator.SetBool("Is_Running", false);

                previousHasArrived = true;

                return;
            }

            currentChaseCooldown += Time.deltaTime;
            if (currentChaseCooldown < chaseCooldown) return;

            animator.SetBool("Is_Running", true);

            if (previousHasArrived) leanAngle.Reset();

            previousHasArrived = false;

            TweenUtil.Update(Time.deltaTime, ref leanAngle);

            pivot.transform.rotation = this.transform.rotation * Quaternion.Euler(leanAngle.value, 0, 0);

            TweenUtil.Update(Time.deltaTime, ref xAxis);
            TweenUtil.Update(Time.deltaTime, ref yAxis);
            TweenUtil.Update(Time.deltaTime, ref zAxis);

            this.transform.position =
                Vector3.right * xAxis.value +
                Vector3.up * (yPosition + yAxis.value) +
                Vector3.forward * zAxis.value;

            Vector3 fwd = new Vector3(target.position.x - this.transform.position.x, 0, target.position.z - this.transform.position.z);

            float alpha = Vector3.SignedAngle(this.transform.forward, fwd, Vector3.up);

            Quaternion q = Quaternion.AngleAxis(alpha * Time.deltaTime, Vector3.up);
            this.transform.rotation *= q;
            //this.transform.LookAt(this.transform.position + fwd);
        }
    }
}