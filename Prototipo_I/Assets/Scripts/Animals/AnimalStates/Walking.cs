using UnityEngine;

public class Walking : AnimalState
{
    public Walking(AnimalAI ai) : base(ai) { }

    public override void OnEnter()
    {
        Vector3 target = animal.GetRandomPositionInCorral;
        animalAI.Agent.SetDestination(target);
    }
    public override void Behaviour()
    {
        if (!animalAI.Agent.pathPending && animalAI.Agent.remainingDistance <= animalAI.Agent.stoppingDistance)
           animalAI.SetState(AnimalAI.State.IDLE);
    }
}
