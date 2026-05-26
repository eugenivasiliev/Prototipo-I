using UnityEngine;
using Utils;

namespace Enemies
{
    public class LadywoolMovement : TweenMovement
    {
        [SerializeField, Range(0, 5)] private float jumpHeight;
        [SerializeField] private GameObject pivot;

        override protected void Start()
        {
            //We only care about y-axis for jump
            xAxis.SetActive(false);
            yAxis.SetActive(true);
            zAxis.SetActive(false);
        }

        void Update()
        {
            TweenUtil.Update(Time.deltaTime, ref yAxis);

            this.transform.position = pivot.transform.position + yAxis.value * jumpHeight * Vector3.up;
            this.transform.rotation = Quaternion.AngleAxis(pivot.transform.rotation.eulerAngles.y, Vector3.up);
        }
    }
}