using UnityEngine;

[CreateAssetMenu(fileName = "PlantsOfTypeCollected", menuName = "Scriptable Objects/Objectives/PlantsOfTypeCollected")]
public class PlantsOfTypeCollected : Objective<string>
{
    [SerializeField] private string plantName;
    [SerializeField] private int plantsToCollect;
    [SerializeField] private int plantsCollected = 0;

    public override bool CheckObjective()
    {
        return plantsCollected >= plantsToCollect;
    }

    public override void UpdateObjective(string plantName)
    {
        if(plantName == this.plantName) plantsCollected++;
    }
}
