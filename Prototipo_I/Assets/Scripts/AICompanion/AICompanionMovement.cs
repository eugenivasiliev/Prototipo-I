using UnityEngine;
using Utils;

namespace AICompanion
{
    public class AICompanionMovement : TweenMovement
    {
        [SerializeField] private Transform playerTransform;
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

            this.transform.position = playerTransform.position +
                -playerTransform.right * separation +
                playerTransform.up * soaringAmplitude * yAxis.value +
                playerTransform.forward * forwardMovement * zAxis.value;

            this.transform.LookAt(this.transform.position + playerTransform.forward);

            if (yAxis.value == yAxis.duration)
                yAxis.Reverse();

            if (zAxis.value == zAxis.duration)
                zAxis.Reverse();
        }
    }
}