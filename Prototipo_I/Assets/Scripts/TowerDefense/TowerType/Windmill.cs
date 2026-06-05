using System.Collections.Generic;
using Farm;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace TowerDefense
{
    public class Windmill : Tower
    {
        private System.Action<float> Give;

        [SerializeField] private List<Plot> plots = new List<Plot>();

        [Header("Loot")]
        [SerializeField] protected GameObject seedLoot;
        private void Start()
        {
            Give += DropLootItem;
            DayNightCycle.Instance.SubscribeTimedEvent(Give, 2);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<Plot>(out Plot plot))
            {
                plots.Add(plot);
            }
        }

        void DropLootItem(float t)
        {
            foreach (Plot plot in plots)
                if (plot.IsPlanted)
                    Instantiate(seedLoot, this.transform.position, Quaternion.identity);

            DayNightCycle.Instance.SubscribeTimedEvent(Give, 2);
        }
    }
}