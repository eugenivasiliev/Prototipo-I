using Items;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace TowerDefense
{
    public class Windmill : MonoBehaviour
    {
        private UnityEvent<float> Give = new UnityEvent<float>();
        int amount = 1;
        private void Start()
        {
            Give.AddListener(AddSeeds);
            DayNightCycle.Instance.SubscribeTimedEvent(Give, 1);
        }

        void AddSeeds(float ff)
        {
            Inventory.Inventory.Instance.AddItem(new FirePlantItem(), 30, out int amountDone);
            DayNightCycle.Instance.SubscribeTimedEvent(Give, amount);
        }
    }
}