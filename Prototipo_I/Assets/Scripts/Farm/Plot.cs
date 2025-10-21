using System.Collections.Generic;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plot : MonoBehaviour, IInteractable
{
    Plant plant;
    PlantData plantData;

    private bool hasWater;
    private bool isFertilized;
    private GameObject currentPlant;

    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] public PlantData plantInfo;

    public bool IsPlanted { get { return plant != null; } }

    public void Plant(PlantData data)
    {
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

        this.plant.Create();
        Debug.Log($"Planta {plant.Name} plantada!");
    }

    public void Fertilize()
    {
        if (!IsPlanted || isFertilized) { Debug.Log("Ya esta fertilizada"); return; }
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

        Inventory.Instance.AddItem(new Item1(), 2, out int amountDone);

        Destroy(currentPlant);
        Debug.Log("Yw. Harvested");

        this.plant = null;
        currentPlant = null;
    }

    public void UpdateUI()
    {
        if (statusText == null || !statusText.gameObject.activeInHierarchy) return;
        if (!IsPlanted)
        {
            statusText.text = "Hueco";
            return;
        }

        statusText.text = $"{plant.Name}\n" +
                          $"Time to Grow: {plant.TimeLeft:F1}s\n" +
                          $"Watered: {(hasWater ? "Sí" : "No")}\n" +
                          $"Fertilizada: {(isFertilized ? "Sí" : "No")}\n";
    }

    private void OnPlantStageChanged(int currentStage)
    {
        hasWater = false;
        isFertilized = false;

        if (currentPlant != null) { Destroy(currentPlant); }

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
        hasWater = true;
        Debug.Log("Regada");
    }

    private void Action_Plant(InputAction.CallbackContext ctx)
    {
        Inventory.Instance.UseCurrentItem(this.gameObject);
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

    public void OnInteract() {}
}
