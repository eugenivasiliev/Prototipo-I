using System;
using UnityEngine;
using UnityEngine.Events;
public class Plant
{
    private int maxStage;

    public int MaxStage => maxStage;

    private int currentStage;

    public int CurrentStage => currentStage;

    private float timeToGrow;
    private float growthTimer;
    private bool isFertilize;


    public string Name { get; private set; }
    public bool IsFullyGrown { get { return currentStage >= maxStage - 1; } }
    public float TimeLeft { get { return Mathf.Max(timeToGrow - growthTimer, 0f); } }

    public Action <int> OnStageChanged;

    private UnityEvent<float> Grow = new UnityEvent<float>();

    public Plant(PlantData data)
    {
        Name = data.plantName;
        maxStage = data.stages.Length;
        timeToGrow = data.timeToGrow;
        currentStage = 0;
        isFertilize = false;
        growthTimer = 0f;
    }

    public void ApplyFertilize(bool isFertilized)
    {
        isFertilize = isFertilized;
    }

    public void TryGrow(float currentTime)
    {
        Grow.AddListener(NextGrowStage);
        DayNightCycle.Instance.SubscribeTimedEvent(Grow, currentTime + timeToGrow / (isFertilize ? 1.5f : 1f));
    }

    public void UpdateGrowth(float deltaTime)
    {
        growthTimer += deltaTime;
    }

    private void NextGrowStage(float time)
    {
        Debug.Log($"{IsFullyGrown}");
        Debug.Log($"{currentStage}");
        if (IsFullyGrown)
        {
            Debug.Log("Ya no puede crecer mas");
            return;
        }

        currentStage++;
        growthTimer = 0f;
        isFertilize = false;
        OnStageChanged?.Invoke(currentStage);
        TryGrow(DayNightCycle.Instance.TotalTime);
        Debug.Log("Sigue creciendo");

    }

    public void FullGrow() {

        currentStage = maxStage;
        Debug.Log(CurrentStage);
    }
}