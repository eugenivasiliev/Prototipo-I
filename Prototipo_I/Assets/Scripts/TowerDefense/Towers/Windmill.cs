using System.Collections.Generic;
using Farm;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace TowerDefense
{
    public class Windmill : Tower
    {
        private UnityEvent<float> Give = new UnityEvent<float>();

        [SerializeField] private List<Plot> plots = new List<Plot>();

        private void Start()
        {
            Give.AddListener(AddSeeds);
            DayNightCycle.Instance.SubscribeTimedEvent(Give, 1);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<Plot>(out Plot plot))
            {
                plots.Add(plot);
            }
        }

        void AddSeeds(float ff)
        {
            foreach (Plot plot in plots)
                if (plot.IsPlanted) Inventory.Inventory.Instance.AddSeeds(1);
            
            DayNightCycle.Instance.SubscribeTimedEvent(Give, 1);
        }
    }
}