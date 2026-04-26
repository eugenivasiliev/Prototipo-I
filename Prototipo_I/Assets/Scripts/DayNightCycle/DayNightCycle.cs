using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using TimedEvent = System.Action<float>;
using Utils;

namespace Utils
{
    /// <summary>
    /// Uses <b>System.Collections.Generic.PriorityQueue</b> for DayTime-dependent events
    /// </summary>
    public class DayNightCycle : Singleton<DayNightCycle>
    {

        [SerializeField] private int dayTime = 0;
        public int DayTime { get { return dayTime; } }

        [SerializeField] private int dayDuration = 2;
        public int DayDuration { get { return dayDuration; } }

        [SerializeField] private int dayCount = 0;
        public int DayCount { get { return dayCount; } }

        public int TotalTime { get => dayTime + dayCount * dayDuration; }

        [SerializeField] private PriorityQueue<TimedEvent, float> timedEvents = new PriorityQueue<TimedEvent, float>();

        private void Awake()
        {
            InitSingleton();
        }

        public void PassTime()
        {
            dayTime++;

            if (dayTime >= dayDuration)
            {
                dayCount++;
                dayTime = 0;
            }

            while (timedEvents.Count > 0 && TotalTime >= timedEvents.PeekPriority())
            {
                TimedEvent nextEvent = timedEvents.Dequeue();
                nextEvent.Invoke(dayTime);
            }
        }

        public void SubscribeTimedEvent(TimedEvent timedEvent, int inCycles) =>
            timedEvents.Enqueue(timedEvent, TotalTime + inCycles);

        public void UnsubscribeTimedEvent(TimedEvent timedEvent, out TimedEvent removedElement, out float priority) =>
            timedEvents.Remove(timedEvent, out removedElement, out priority);
    }
}