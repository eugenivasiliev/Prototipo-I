using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemieManager : MonoBehaviour
{
    private static PlotManager Instance;

    [SerializeField] private GameObject enemie;
    [SerializeField] private short enemiesByZone;
    [SerializeField] private List<Transform> spawnZones = new List<Transform>();

    private float halfDayTime;

    private UnityEvent<float> Spawn = new UnityEvent<float>();

    void Start()
    {
        halfDayTime = DayNightCycle.Instance.DayDuration / 2;
        Debug.Log(halfDayTime);
        Spawn.AddListener(SpawnEnemies);

        DayNightCycle.Instance.SubscribeTimedEvent(Spawn, DayNightCycle.Instance.DayDuration - halfDayTime);
    }

    private void SpawnEnemies(float usseless)
    {
        if (enemie == null) { return; }

        foreach (Transform zone in spawnZones)
        {
                for (int i = 0; i < enemiesByZone; i++)
                {
                    Instantiate(enemie, zone.position, zone.rotation);
                }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
