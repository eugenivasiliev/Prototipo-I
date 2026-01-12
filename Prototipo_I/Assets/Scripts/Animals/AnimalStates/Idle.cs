using UnityEngine;

public class Idle : AnimalState
{
    private float timer = 0f;
    public Idle(AnimalAI ai) : base(ai) { }

    public override void Behaviour()
    {

        timer += Time.deltaTime;

        if (timer >= 5f)
        {
            timer = 0f;
            AudioManager.instance.PlayMusic("AnimalSound");
        }
        if (animal.IsHungry)
            animalAI.SetState(AnimalAI.State.EATING);
        if(animal.canBreed)
            animalAI.SetState(AnimalAI.State.BREEDING);
    }
}
