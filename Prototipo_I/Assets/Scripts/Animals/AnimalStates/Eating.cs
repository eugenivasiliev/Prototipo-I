using UnityEngine;

public class Eating : AnimalState
{
    public Eating(AnimalAI ai) : base(ai) { }

    public override void Behaviour()
    {
        Debug.Log("Comiendo");
        animal.Feeding();
        animalAI.SetState(AnimalAI.State.IDLE);

    }
}
