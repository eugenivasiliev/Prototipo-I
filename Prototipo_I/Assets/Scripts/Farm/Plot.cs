using System.Collections.Generic;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plot : MonoBehaviour, IInteractable, IDamageable
{
    Plant plant;
    PlantData plantData;

    private bool hasWater;
    private bool isFertilized;
    private GameObject currentPlant;

    [SerializeField] private TextMeshProUGUI statusText;
    public TextMeshProUGUI StatusText => statusText;
    [SerializeField] public PlantData plantInfo;

    public bool IsPlanted { get { return plant != null; } }

    [SerializeField] private int health;
    public int Health { get => health; set => health = value; }
    public int MaxHealth { get => 100; set { } }

    private void Awake()
    {
        if(statusText != null)
            statusText.gameObject.SetActive(false);
    }

    public void Plant(PlantData data)
    {
        AudioManager.instance.PlaySFX("Plant");
        if (IsPlanted)
        {
            if (PlotManager.Instance.HybridationManager.TryFindHybrid((plantData, data), out PlantData newPlant))
            {
                this.plantData = newPlant;
                plant = new Plant(newPlant);
            }
            else
            {
                Debug.Log("Already planted");
                return;
            }
        }
        else
        {
            plantData = data;
            plant = new Plant(data);
        }

        currentPlant = Instantiate(plantData.stages[0], transform.position, Quaternion.Euler(-90, 0, 0), transform);

        plant.OnStageChanged += OnPlantStageChanged;

        hasWater = false;
        isFertilized = false;

        Debug.Log($"Planta {plant.Name} plantada!");
    }

    public void Fertilize()
    {
        if (!IsPlanted || isFertilized) { Debug.Log("Ya esta fertilizada"); return; }
        AudioManager.instance.PlaySFX("Fertilize");
        isFertilized = true;
        plant.ApplyFertilize(isFertilized);
    }
    private void Harvest()
    {
        if (!IsPlanted || !plant.IsFullyGrown)
        {
            Debug.Log("Aun no esta lista");
            return;
        }

        AudioManager.instance.PlaySFX("Harvesting");
        Inventory.Instance.AddItem(new GasPlantItem(), 3, out int amountDone);
        Debug.Log("Add: " + amountDone);

        if(ObjectivesManager.Instance.TryGetObjective<PlantsOfTypeCollected, string>(out List<PlantsOfTypeCollected> objs))
        {
            foreach(PlantsOfTypeCollected obj in objs)
            {
                obj.UpdateObjective(plantData.plantName);
            }
        }

        Destroy(currentPlant);
        Debug.Log("Yw. Harvested");

        this.plant = null;
        currentPlant = null;
    }






    public void UpdateUI()
    {
        if ((this as IDamageable).IsDead())
        {
            Destroy(currentPlant);
            this.plant = null;
            currentPlant = null;
        }
        statusText.gameObject.SetActive(false);
        if (statusText == null || !statusText.gameObject.activeInHierarchy) return;
        if (!IsPlanted)
        {
            statusText.text = "Hueco";
            return;
        }
        plant.UpdateGrowth(Time.deltaTime);
        if (!hasWater && plant.TimeLeft <= 0f)
        {
            statusText.text = "<size=40><color=red>Necesita Agua</color></size>";
            return;
        }
        statusText.text = $"{plant.Name}\n" +
                          $"Stage: {plant.CurrentStage} / 2\n" +
                          $"Time to next stage: {plant.TimeLeft:F1}s\n" +
                          $"Watered: {(hasWater ? "Sí" : "No")}\n" +
                          $"Fertilized: {(isFertilized ? "Sí" : "No")}";
    }

    private void OnPlantStageChanged(int currentStage)
    {
        hasWater = false;
        isFertilized = false;

        if (currentPlant != null) { Destroy(currentPlant); }

        AudioManager.instance.PlaySFX("NextStage");
        GameObject prefab = plantData.stages[currentStage];
        currentPlant = Instantiate(prefab, transform.position, Quaternion.Euler(-90, 0, 0), transform);

    }

    public List<IInteractable.KeyBinding> keyBindings => new List<IInteractable.KeyBinding>{
    new IInteractable.KeyBinding("water", InputActionChange.ActionCanceled, Action_Water),
    new IInteractable.KeyBinding("plant", InputActionChange.ActionCanceled, Action_Plant),
    new IInteractable.KeyBinding("harvest", InputActionChange.ActionCanceled, Action_Harvest),
    new IInteractable.KeyBinding("fertilize", InputActionChange.ActionCanceled, Action_Fertilize)
    };

    private void Action_Water(InputAction.CallbackContext ctx)
    {
        AudioManager.instance.PlaySFX("Water");
        hasWater = true;
        plant.TryGrow(DayNightCycle.Instance.TotalTime, hasWater);
        Debug.Log("Regada");
    }

    private void Action_Plant(InputAction.CallbackContext ctx)
    {
        Item item = Inventory.Instance.GetCurrentItem();
        if (item != null && item is IPlantSeed)
        {
            this.Plant((item as IPlantSeed).PlantData);
            Inventory.Instance.RemoveItem(item);
        } 
    }

    private void Action_Harvest(InputAction.CallbackContext ctx)
    {
        if (IsPlanted && plant.IsFullyGrown)
        {
            Harvest();
            Debug.Log("Coshechada");
        }
    }

    private void Action_Fertilize(InputAction.CallbackContext ctx)
    {
        Fertilize();
        Debug.Log("Fertilizada");
    }
    public void FullGrow() {
        if (plant != null)
            plant.FullGrow();
        else
            return;

        OnPlantStageChanged(plant.CurrentStage -1);
    }
    public void OnInteract() {}
}
