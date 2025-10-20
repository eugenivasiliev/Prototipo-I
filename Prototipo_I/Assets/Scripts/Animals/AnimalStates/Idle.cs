using UnityEngine;

public class Idle : AnimalState
{
    public Idle(AnimalAI ai) : base(ai) { }

    public override void Behaviour()
    {
        if(animal.IsHungry)
            animalAI.SetState(AnimalAI.State.EATING);
        if(animal.canBreed)
            animalAI.SetState(AnimalAI.State.BREEDING);
    }
}
