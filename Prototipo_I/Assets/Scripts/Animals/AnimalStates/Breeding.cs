using UnityEngine;

public class Breeding : AnimalState
{
    private Animal targetMate;
    private float breedDistance = 1f;
    public Breeding(AnimalAI ai) : base(ai) { }

    public override void Behaviour()
    {
        if (animal.canBreed)
        {
            if (targetMate == null)
            {
                targetMate = animal.FindMate(50f);
            }

            if (targetMate != null)
            {
                animalAI.Agent.SetDestination(targetMate.transform.position);
                float distance = Vector3.Distance(Animal.transform.position, targetMate.transform.position);

                if (distance <= breedDistance)
                {
                    BreedWith(targetMate);
                    targetMate = null;
                }
            }
        }
        if(!animal.canBreed)
            animalAI.SetState(AnimalAI.State.IDLE);
    }

    private void BreedWith(Animal mate)
    {
        if (animal.ChildPrefab == null) return;

        Vector3 spawnPos = (Animal.transform.position + mate.transform.position) / 2f;
        GameObject baby = GameObject.Instantiate(animal.ChildPrefab, spawnPos, Quaternion.identity);

        Animal babyAnimal = baby.GetComponent<Animal>();
        if (babyAnimal != null)
        {
            babyAnimal.SetInitialValues(0);
        }

        animal.ResetBreedStatus();
        mate.ResetBreedStatus();

        Debug.Log("¡Se ha reproducido un nuevo animal!");
    }
}
