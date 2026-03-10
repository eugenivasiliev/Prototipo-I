using UnityEngine;

namespace Objectives
{
    [CreateAssetMenu(fileName = "PlantsCollected", menuName = "Scriptable Objects/Objectives/PlantsCollected")]
    public class PlantsCollected : Objective<int>
    {
        [SerializeField] private string plantName;
        [SerializeField] private int plantsToCollect;
        [SerializeField] private int plantsCollected = 0;

        public override string Text => "Collect: " + plantsCollected + "/" + plantsToCollect + "\n";

        public override bool CheckObjective()
        {
            return plantsCollected >= plantsToCollect;
        }

        public override void Init()
        {
            plantsCollected = 0;
        }

        public override void UpdateObjective(int amount)
        {
            plantsCollected += amount;
        }
    }
}