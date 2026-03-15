using UnityEngine;

namespace Objectives
{
    [CreateAssetMenu(fileName = "WavesCompleted", menuName = "Scriptable Objects/Objectives/WavesCompleted")]
    public class WavesCompleted : Objective<int>
    {
        [SerializeField] private int wavesToComplete;
        [SerializeField] private int wavesCompleted = 0;

        public override string Text => "Survive waves: " + wavesCompleted + "/" + wavesToComplete + "\n";

        public override bool CheckObjective()
        {
            return wavesCompleted >= wavesToComplete;
        }

        public override void Init()
        {
            wavesCompleted = 0;
        }

        public override void UpdateObjective(int numberCompleted)
        {
            wavesCompleted += numberCompleted;
        }
    }
}