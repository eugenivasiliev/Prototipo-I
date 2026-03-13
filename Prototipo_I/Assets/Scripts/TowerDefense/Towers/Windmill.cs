using Items;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace TowerDefense
{
    public class Windmill : MonoBehaviour
    {
        private UnityEvent<float> Give = new UnityEvent<float>();
        [SerializeField] private int seedsPerRound = 3;
        int amount = 1;
        private void Start()
        {
            Give.AddListener(AddSeeds);
            DayNightCycle.Instance.SubscribeTimedEvent(Give, 1);
        }

        void AddSeeds(float ff)
        {
            Inventory.Inventory.Instance.AddSeeds(seedsPerRound);
            DayNightCycle.Instance.SubscribeTimedEvent(Give, amount);
        }
    }
}