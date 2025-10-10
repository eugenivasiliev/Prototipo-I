using UnityEngine;
using UnityEngine.Events;

public class DayTimeTest : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] private float Time;
    private UnityEvent<float> e = new UnityEvent<float>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        e.AddListener(MyEvent);

        DayNightCycle.Instance.SubscribeTimedEvent(e, Time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MyEvent(float t)
    {
        Debug.Log("Hello " + t);
    }
}
