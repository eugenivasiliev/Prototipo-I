using UnityEngine;

namespace Utils
{
    public class TweenMovement : MonoBehaviour
    {
        public Tween<float> xAxis;
        public Tween<float> yAxis;
        public Tween<float> zAxis;

        void Start()
        {
            xAxis.SetActive(true);
            yAxis.SetActive(true);
            zAxis.SetActive(true);
        }

        void Update()
        {
            if (TweenUtil.Update(Time.deltaTime, ref xAxis) && 
                TweenUtil.Update(Time.deltaTime, ref yAxis) && 
                TweenUtil.Update(Time.deltaTime, ref zAxis)) 
                this.transform.position = new Vector3(xAxis.value, yAxis.value, zAxis.value);
        }
    }
}