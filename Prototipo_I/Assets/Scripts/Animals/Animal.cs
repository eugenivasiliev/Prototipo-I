using UnityEngine;
using UnityEngine.Events;
public class Animal : MonoBehaviour
{
    [SerializeField] private float eatInterval = 30f;
    [SerializeField] private int mealsForProduction = 3;
    [SerializeField] private int mealsForGrow = 5;
    [SerializeField] private float breedingCooldown = 300f;
    [SerializeField] private int maxMealsEaten = 8;
    [SerializeField] private GameObject grownPrefab;

    public bool CanBreed = false;

    private int maxCaring;
    private int caring = 0;
    private int mealsEaten = 0;

    private bool isHungry = false;
    private bool isGrow = false;
    private bool canCollet = false;

    private UnityEvent<float> feed = new UnityEvent<float> ();
    private UnityEvent<float> breed = new UnityEvent<float> ();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxCaring = mealsForGrow + mealsForProduction;
        feed.AddListener(needFeed);
        breed.AddListener(Breeding);


        DayNightCycle.Instance.SubscribeTimedEvent(feed,DayNightCycle.Instance.TotalTime + eatInterval);
        DayNightCycle.Instance.SubscribeTimedEvent(breed,DayNightCycle.Instance.TotalTime + breedingCooldown);
    }

    private void needFeed(float unused)
    {
        Debug.Log("necesita comida");

        isHungry = true;
    }

    private void Feeding()
    {
        if (mealsEaten == maxMealsEaten)
        {
            Debug.Log("Can't eat more");
            return;
        }
        caring++;
        mealsEaten++;
        DayNightCycle.Instance.SubscribeTimedEvent(feed, DayNightCycle.Instance.TotalTime + eatInterval);

        Debug.Log("Veces comido" + mealsEaten);

        if (mealsEaten >= mealsForProduction && !canCollet)
        {
            canCollet = true;
        }

        if (mealsEaten >= mealsForGrow && !isGrow)
        {
            isGrow = true;
            Debug.Log("Listo para crecer");
            Grow();
        }

        isHungry = false;
    }



    private void Collect()
    {
        caring++;
        Debug.Log("Ya estoy listo para recolectar");
        mealsEaten = 0;
        canCollet = false;
    }

    private void Grow()
    {
        if(grownPrefab != null)
        {
            caring += 2;
            GameObject newAnimal = Instantiate(grownPrefab, transform.position, transform.rotation);

            Animal newAnimalScript = newAnimal.GetComponent<Animal>();
            if(newAnimalScript != null )
            {
                newAnimalScript.SetInitialValues(caring);
            }

            Destroy(gameObject);
        }
    }

    public void SetInitialValues(int oldCaring) { caring = oldCaring; }

    private void Breeding(float unused)
    {
        if(caring >= maxCaring)
        {
            Debug.Log("Pueden tener babies");
            canBreed = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && isHungry)
        {
            Feeding();
        }
        if (Input.GetKeyDown(KeyCode.R) && canCollet)
        {
            Collect();
        }
    }
}
