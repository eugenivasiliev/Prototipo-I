using UnityEngine;
using Utils;

namespace AICompanion
{
    public class AICompanionMovement : TweenMovement
    {
        [SerializeField, Range(0, 5)] private float soaringAmplitude;
        [SerializeField, Range(0, 5)] private float separation;
        [SerializeField, Range(0, 5)] private float forwardMovement;

        override protected void Start()
        {
            yAxis.SetActive(true);
            zAxis.SetActive(true);
        }

        void Update()
        {
            TweenUtil.Update(Time.deltaTime, ref yAxis);
            TweenUtil.Update(Time.deltaTime, ref zAxis);

            this.transform.localPosition = 
                -Vector3.right * separation +
                Vector3.up * soaringAmplitude * yAxis.value +
                Vector3.forward * forwardMovement * zAxis.value;

            if (yAxis.value == yAxis.duration)
                yAxis.Reverse();

            if (zAxis.value == zAxis.duration)
                zAxis.Reverse();
        }
    }
}