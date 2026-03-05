using System.Collections.Generic;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plot : MonoBehaviour, IInteractable, IDamageable
{
    [SerializeField] private HybridationManager hybridationManager;

    Plant plant;
    PlantData plantData;

    private bool isFertilized;
    private GameObject currentPlant;

    [SerializeField] private TextMeshProUGUI statusText;
    public TextMeshProUGUI StatusText => statusText;
    [SerializeField] public PlantData plantInfo;

    public bool IsPlanted { get { return plant != null; } }

    [SerializeField] private int health;
    public int Health { get => health; set => health = value; }
    public int MaxHealth { get => 100; set { } }


    public GameObject plantingFeedback;
    public GameObject ripeFeedback;
    private GameObject ripeParticles;
    public GameObject harvestFeedback;

    private void Awake()
    {
        if(statusText != null)
            statusText.gameObject.SetActive(false);

        health = 1;
    }

    public void Plant(PlantData data)
    {
        AudioManager.instance.PlaySFX("Plant");
        if (IsPlanted)
        {
            if (hybridationManager.TryFindHybrid((plantData, data), out PlantData newPlant))
            {
                this.plantData = newPlant;
                plant = new Plant(newPlant);
            }
            else return;
        }
        else
        {
            plantData = data;
            plant = new Plant(data);
        }

        currentPlant = Instantiate(plantData.stages[0], transform.position, Quaternion.Euler(-90, 0, 0), transform);

        plant.OnStageChanged += OnPlantStageChanged;

        isFertilized = false;

        Instantiate(plantingFeedback, transform.position, Quaternion.identity, transform);        
    }

    public void Fertilize()
    {
        if (!IsPlanted || isFertilized) return;
        AudioManager.instance.PlaySFX("Fertilize");
        isFertilized = true;
        plant.ApplyFertilize(isFertilized);
    }
    private void Harvest()
    {
        if (!IsPlanted || !plant.IsFullyGrown) return;

        AudioManager.instance.PlaySFX("Harvesting");
        Inventory.Instance.AddItem(new GasPlantItem(), 3, out int amountDone);

        if(ObjectivesManager.Instance.TryGetObjective<PlantsOfTypeCollected, string>(out List<PlantsOfTypeCollected> objs))
        {
            foreach(PlantsOfTypeCollected obj in objs)
            {
                obj.UpdateObjective(plantData.plantName);
            }
        }

        Destroy(currentPlant);

        this.plant = null;
        currentPlant = null;
    }

    private void OnPlantStageChanged(int currentStage)
    {
        isFertilized = false;

        if (currentPlant != null) { Destroy(currentPlant); }

        AudioManager.instance.PlaySFX("NextStage");
        GameObject prefab = plantData.stages[currentStage];
        currentPlant = Instantiate(prefab, transform.position, Quaternion.Euler(-90, 0, 0), transform);

    }

    public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding>{
    new IInteractable.KeyBinding("plant", InputActionChange.ActionCanceled, Action_Plant),
    new IInteractable.KeyBinding("harvest", InputActionChange.ActionCanceled, Action_Harvest),
    new IInteractable.KeyBinding("fertilize", InputActionChange.ActionCanceled, Action_Fertilize)
    };

    private void Action_Plant(InputAction.CallbackContext ctx)
    {
        Item item = Inventory.Instance.GetCurrentItem();
        if (item != null && item is IPlantSeed)
        {
            this.Plant((item as IPlantSeed).PlantData);
            Inventory.Instance.RemoveItem(item);
            plant.TryGrow(DayNightCycle.Instance.TotalTime);
        } 
    }

    private void Action_Harvest(InputAction.CallbackContext ctx)
    {
        if (IsPlanted && plant.IsFullyGrown)
            Harvest();

        Destroy(ripeParticles);
    }

    private void Action_Fertilize(InputAction.CallbackContext ctx)
    {
        Fertilize();
    }
    public void FullGrow() {
        if (plant != null)
            plant.FullGrow();
        else
            return;

        OnPlantStageChanged(plant.CurrentStage -1);

        ripeParticles = Instantiate(ripeFeedback, transform.position, Quaternion.identity, transform);
    }
    public void OnInteract() {}
}