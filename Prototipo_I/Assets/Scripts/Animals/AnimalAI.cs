using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AnimalAI : MonoBehaviour
{
    public enum State
    {
        IDLE,
        EATING,
        WALKING,
        BREEDING
    }

    private Animal animal;

    [SerializeField] private AnimalState animalState;
    [SerializeField] private float minTimeToWalk;
    [SerializeField] private float maxTimeToWalk;

    private float timeToWalk;
    private NavMeshAgent agent;
    public NavMeshAgent Agent { get => agent; }

    private UnityEvent<float> walk = new UnityEvent<float>();

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        animalState = new Idle(this);
        timeToWalk = Random.Range(minTimeToWalk, maxTimeToWalk);

        walk.AddListener(Walking);

        DayNightCycle.Instance.SubscribeTimedEvent(walk, DayNightCycle.Instance.TotalTime + timeToWalk);
    }

    private void Update()
    {
        animalState.Behaviour();
        Debug.Log(animalState);
    }

    public void SetState(State newState)
    {
        switch (newState)
        {
            case State.IDLE:
                animalState = new Idle(this);
                break;
            case State.EATING:
                animalState = new Eating(this);
                break;
            case State.WALKING:
                animalState = new Walking(this);
                break;
            case State.BREEDING:
                animalState = new Breeding(this);
                break;
            default:
                break;
        }
        animalState.Animal = this;
        animalState?.OnEnter();
    }

    private void Walking(float t)
    {
        SetState(State.WALKING);

        timeToWalk = Random.Range(minTimeToWalk, maxTimeToWalk);
        DayNightCycle.Instance.SubscribeTimedEvent(walk, DayNightCycle.Instance.TotalTime + timeToWalk);
    }

}
