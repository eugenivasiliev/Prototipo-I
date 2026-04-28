using UnityEngine;

namespace Utils
{
    public class SingleRoundObject : MonoBehaviour
    {
        void Start()
        {
            DayNightCycle.Instance.SubscribeTimedEvent(Disappear, 1);
        }

        private void Disappear(float t) =>
            Destroy(this.gameObject);
    }
}