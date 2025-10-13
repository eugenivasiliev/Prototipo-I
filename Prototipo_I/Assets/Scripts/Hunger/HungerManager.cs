using UnityEngine;
using UnityEngine.Events;

public class HungerManager : MonoBehaviour
{
    private int hunger;
    private int maxHunger;
    private float timeToHunger;
    private int sprintThreshold;
    private int damageAmount;

    private UnityEvent<float> onHunger;

    public void Eat(int amount) => hunger = Mathf.Min(hunger + amount, maxHunger);
    public void EatPercent(float percent) => Eat((int)(maxHunger * percent / 100.0f));
    public void EatMax() => hunger = maxHunger;

    public void Hunger(int amount) => hunger = Mathf.Max(hunger - amount, 0);
    public void Hunger() => Hunger(1);
    public void HungerPercent(float percent) => Hunger((int)(maxHunger * percent / 100.0f));
    public void HungerMax() => hunger = 0;

    public bool CanSprint { get => hunger >= sprintThreshold; }
    public bool IsBeingDamaged { get => hunger <= 0; }

    public void Start()
    {
        onHunger.AddListener(OnHunger);
        DayNightCycle.Instance.SubscribeTimedEvent(onHunger, DayNightCycle.Instance.DayTime + timeToHunger);
    }

    public void OnHunger(float dayTime)
    {
        Hunger();
        if (IsBeingDamaged && TryGetComponent(out IDamageable damageable)) 
            damageable.Damage(damageAmount);
    }
}
