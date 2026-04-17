using UnityEngine;
using Utils;

namespace Enemies
{
    public class LadywoolMovement : TweenMovement
    {
        [SerializeField, Range(0, 5)] private float jumpHeight;

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

            this.transform.localPosition = new Vector3(this.transform.localPosition.x, yAxis.value * jumpHeight, this.transform.localPosition.z);
        }
    }
}