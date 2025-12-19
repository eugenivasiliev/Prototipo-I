using UnityEngine;

public abstract class AnimalState
{
    protected AnimalAI animalAI;
    protected Animal animal;
    protected AnimalState(AnimalAI ai)
    {
        animalAI = ai;
        animal = ai.GetComponent<Animal>();
    }
    public AnimalAI Animal { get => animalAI; set => animalAI = value; }

    public virtual void OnEnter() { }
    public abstract void Behaviour();
}
