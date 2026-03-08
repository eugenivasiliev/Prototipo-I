using UnityEngine;
using UnityEngine.Events;

public class Base : MonoBehaviour, IDamageable
{
    public static Base instance;

    [SerializeField] private int health;

    public int Health { get => health; set => health = value; }
    public int MaxHealth { get => 100; set { } }
    [SerializeField] private Canvas ui_health;


    private UnityEvent<float> BaseProduction = new UnityEvent<float>();
    private void Start()
    {
        health = MaxHealth;
        instance = this;
        BaseProduction.AddListener(AddSeeds);
        DayNightCycle.Instance.SubscribeTimedEvent(BaseProduction, 1);
    }

    void Update() {
        if (health <= 0.0f)
        {

            EnemyManager.Instance.ReturnToSpawn(0.0f);
            health = MaxHealth;
        }
    }

    void AddSeeds(float ff)
    {
        //Inventory.Instance.AddItem(new FirePlantItem(), 30, out int amountDone);
        Inventory.Instance.RemoveItem(new FirePlantItem());
        DayNightCycle.Instance.SubscribeTimedEvent(BaseProduction, 1);
    }


}
