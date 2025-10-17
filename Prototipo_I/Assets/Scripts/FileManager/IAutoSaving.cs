using UnityEngine;
using UnityEngine.Events;

public interface IAutoSaving<T> : ISaveable<T>
{
    public float AutoSaveTime { get; }
    public void SetupAutoSave()
    {
        SaveEvent = new UnityEvent<float>();
        SaveEvent.AddListener(Save);
        Debug.Log(SaveEvent.ToString());
        DayNightCycle.Instance.SubscribeTimedEvent(SaveEvent, DayNightCycle.Instance.TotalTime + AutoSaveTime);
    }

    public void Save(float t)
    {
        Save();
        DayNightCycle.Instance.SubscribeTimedEvent(SaveEvent, DayNightCycle.Instance.TotalTime + AutoSaveTime);
    }
}
