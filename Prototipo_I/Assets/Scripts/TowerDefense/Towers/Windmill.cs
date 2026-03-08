using UnityEngine;
using UnityEngine.Events;

public class Windmill : MonoBehaviour
{
    private UnityEvent<float> Give = new UnityEvent<float>();
    private void Start()
    {
        Give.AddListener(AddSeeds);
        DayNightCycle.Instance.SubscribeTimedEvent(Give, 1);
    }

    void AddSeeds(float ff)
    {
        Inventory.Instance.AddItem(new FirePlantItem(), 30, out int amountDone);
        DayNightCycle.Instance.SubscribeTimedEvent(Give, 1);
    }
}
