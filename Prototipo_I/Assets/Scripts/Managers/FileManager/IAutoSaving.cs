using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Saving
{
    public interface IAutoSaving<T> : ISaveable<T>
    {
        public void SetupAutoSave()
        {
            SaveEvent = new UnityEvent<float>();
            SaveEvent.AddListener(Save);
            DayNightCycle.Instance.SubscribeTimedEvent(SaveEvent, 1);
        }

        public void Save(float t)
        {
            Save();
            DayNightCycle.Instance.SubscribeTimedEvent(SaveEvent, 1);
        }
    }
}