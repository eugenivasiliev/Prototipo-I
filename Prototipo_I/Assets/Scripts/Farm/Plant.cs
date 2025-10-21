using System;
using UnityEngine;
using UnityEngine.Events;
public class Plant
{
    private int maxStage;
    private int currentStage;
    private float timeToGrow;
    private bool isFertilize;
    private bool hasWater;

    public string Name { get; private set; }
    public bool IsFullyGrown { get { return currentStage >= maxStage - 1; } }
    public float TimeLeft { get { return Mathf.Max(timeToGrow - DayNightCycle.Instance.DayTime, 0f); } }

    public Action <int> OnStageChanged;

    private UnityEvent<float> Grow = new UnityEvent<float>();

    public Plant(PlantData data)
    {
        Name = data.plantName;
        maxStage = data.stages.Length;
        timeToGrow = data.timeToGrow;
        currentStage = 0;
        isFertilize = false;
        hasWater = false;
    }

    public void Create()
    {
        Grow.AddListener(NextGrowStage);
        
        DayNightCycle.Instance.SubscribeTimedEvent(Grow, DayNightCycle.Instance.TotalTime + timeToGrow);
    }

    public void ApplyFertilize(bool isFertilized)
    {
        isFertilize = isFertilized;
    }

    private void NextGrowStage(float time)
    {
        currentStage++;
        Debug.Log($"{IsFullyGrown}");
        Debug.Log($"{currentStage}");
        if (IsFullyGrown) 
        {
            Debug.Log("Ya no puede crecer mas");
        }
        else
        {
            Debug.Log("Sigue creciendo");
            DayNightCycle.Instance.SubscribeTimedEvent(Grow, DayNightCycle.Instance.TotalTime + (timeToGrow / (isFertilize ? 1.5f : 1f)));
        }

        isFertilize = false;
        OnStageChanged?.Invoke(currentStage);
    }
}