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

    [SerializeField] private PriorityQueue<TimedEvent, float> timedEvents = new PriorityQueue<TimedEvent, float>();

    [Header("Skyboxes")]
    [SerializeField] private List<Material> skyboxes = new List<Material>();
    private Material skybox;

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
        RenderSettings.skybox = skyboxes[0];
        skybox = skyboxes[0];
        foreach(string s in skyboxes[0].GetPropertyNames(MaterialPropertyType.Float)) Debug.Log(s);
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

        skyboxes[0].SetFloat("_AtmosphereThickness", Mathf.Clamp(DayTime / DayDuration * 5, 0, 5));

        //float curDayPhase = dayTime / dayDuration * skyboxes.Count;
        //int curSkyboxIndex = Mathf.FloorToInt(curDayPhase);

        //skybox.Lerp(skyboxes[curSkyboxIndex % skyboxes.Count], skyboxes[(curSkyboxIndex + 1) % skyboxes.Count], curDayPhase - curSkyboxIndex);
        //RenderSettings.skybox = skybox;
        //DynamicGI.UpdateEnvironment();
    }

    public void SubscribeTimedEvent(TimedEvent timedEvent, float time) => 
        timedEvents.Enqueue(timedEvent, time);

    public void UnsubscribeTimedEvent(TimedEvent timedEvent, out TimedEvent removedElement, out float priority) =>
        timedEvents.Remove(timedEvent, out removedElement, out priority);
}
