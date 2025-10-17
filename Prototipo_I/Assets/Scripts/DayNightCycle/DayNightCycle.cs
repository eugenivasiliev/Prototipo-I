using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using TimedEvent = UnityEngine.Events.UnityEvent<float>;

/// <summary>
/// Centralised manager for daytime-dependent events.
/// Reduces comparisons each frame and eliminates corroutines/update methods.
/// </summary>
public class DayNightCycle : MonoBehaviour 
{

    [SerializeField] private static DayNightCycle instance;
    public static DayNightCycle Instance {  get { return instance; } }

    [SerializeField] private float dayTime = 0;
    public float DayTime {  get { return dayTime; } }

    [SerializeField] private float dayDuration = 1;
    public float DayDuration { get { return dayDuration; } }

    [SerializeField] private int dayCount = 0;
    public int DayCount { get { return dayCount; } }

    public float TotalTime { get => dayTime + dayCount * dayDuration; }
    public float DayElapsed01 { get => dayTime / dayDuration; }

    [SerializeField] private PriorityQueue<TimedEvent, float> timedEvents = new PriorityQueue<TimedEvent, float>();

    [Header("Skybox")]
    [SerializeField] private Material skybox;
    [SerializeField] private AnimationCurve atmosphereThickness;
    [SerializeField] private AnimationCurve exposure;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        RenderSettings.skybox = skybox;
    }

    private void Update()
    {
        dayTime += Time.deltaTime;
        if(dayTime > dayDuration)
        {
            dayCount++;
            dayTime = 0;
        }

        while(timedEvents.Count > 0 && TotalTime >= timedEvents.PeekPriority())
        {
            TimedEvent nextEvent = timedEvents.Dequeue();
            nextEvent.Invoke(dayTime);
        }

        skybox.SetFloat("_AtmosphereThickness", atmosphereThickness.Evaluate(DayElapsed01));
        skybox.SetFloat("_Exposure", exposure.Evaluate(DayElapsed01));
    }

    public void SubscribeTimedEvent(TimedEvent timedEvent, float time) => 
        timedEvents.Enqueue(timedEvent, time);

    public void UnsubscribeTimedEvent(TimedEvent timedEvent, out TimedEvent removedElement, out float priority) =>
        timedEvents.Remove(timedEvent, out removedElement, out priority);
}
