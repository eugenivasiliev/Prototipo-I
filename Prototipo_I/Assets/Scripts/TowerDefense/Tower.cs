using System;
using UnityEngine.Events;

public class Tower
{
    private int maxStage;

    public int MaxStage => maxStage;

    private int currentStage;

    public int CurrentStage => currentStage;


    public string Name { get; private set; }
    public bool IsFullyUpgraded { get { return currentStage >= maxStage - 1; } }

    public Action<int> OnStageChanged;

    public Tower(TowerData data)
    {
        Name = data.Name;
        maxStage = data.stages.Length;
        currentStage = 0;
    }
}