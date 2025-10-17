using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public class Animal : MonoBehaviour
{
    [SerializeField] private float eatInterval = 30f;
    [SerializeField] private short mealsForProduction = 3;
    [SerializeField] private short mealsForGrow = 5;
    [SerializeField] private float breedingCooldown = 300f;
    [SerializeField] private short maxMealsEaten = 8;
    [SerializeField] private GameObject grownPrefab;
    [SerializeField] private GameObject corral;

    public bool canBreed { get; private set; } = false;

    private short maxCaring;
    private short caring;
    private short mealsEaten = 0;

    public bool IsHungry { get; private set; } = false;
    private bool canCollet = false;

    private UnityEvent<float> feed = new UnityEvent<float> ();
    private UnityEvent<float> breed = new UnityEvent<float> ();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxCaring = (short)(mealsForGrow + mealsForProduction); //Porque hace esta conversion?


        feed.AddListener(needFeed);
        breed.AddListener(Breeding);


        DayNightCycle.Instance.SubscribeTimedEvent(feed,DayNightCycle.Instance.TotalTime + eatInterval);
        DayNightCycle.Instance.SubscribeTimedEvent(breed,DayNightCycle.Instance.TotalTime + breedingCooldown);
    }

    private void needFeed(float t)
    {
        Debug.Log("necesita comida");

        IsHungry = true;
    }

    public void Feeding()
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

        if (mealsEaten >= mealsForProduction)
        {
            canCollet = true;
        }

        if (mealsEaten >= mealsForGrow)
        {
            Debug.Log("Listo para crecer");
            Grow();
        }

        IsHungry = false;
    }

    public Vector3 GetRandomPositionInCorral
    {
        get
        {
            if (corral == null) return transform.position;

            Collider col = corral.GetComponent<Collider>();

            Vector3 min = col.bounds.min;
            Vector3 max = col.bounds.max;

            float x = Random.Range(min.x, max.x);
            float y = transform.position.y;
            float z = Random.Range(min.z, max.z);

            return new Vector3(x, y, z);
        }
        
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

    public void SetInitialValues(short oldCaring) { caring = oldCaring; }

    private void Breeding(float t)
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
        if(Input.GetKeyDown(KeyCode.E) && IsHungry)
        {
            Feeding();
        }
        if (Input.GetKeyDown(KeyCode.R) && canCollet)
        {
            Collect();
        }
    }
}
