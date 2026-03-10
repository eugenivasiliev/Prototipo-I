using UnityEngine;

namespace Objectives
{
    [CreateAssetMenu(fileName = "PlantsOfTypeCollected", menuName = "Scriptable Objects/Objectives/PlantsOfTypeCollected")]
    public class PlantsOfTypeCollected : Objective<string>
    {
        [SerializeField] private string plantName;
        [SerializeField] private int plantsToCollect;
        [SerializeField] private int plantsCollected = 0;

        public override string Text => "Collect " + plantName + ": " + plantsCollected + "/" + plantsToCollect + "\n";

        public override bool CheckObjective()
        {
            return plantsCollected >= plantsToCollect;
        }

        public override void Init()
        {
            plantsCollected = 0;
        }

        public override void UpdateObjective(string plantName)
        {
            if (plantName == this.plantName) plantsCollected++;
        }
    }
}