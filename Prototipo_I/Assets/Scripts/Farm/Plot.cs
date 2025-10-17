using System.Timers;
using TMPro;
using UnityEngine;

public class Plot : MonoBehaviour
{
    Plant plant;
    PlantData plantData;

    private bool hasWater;
    private bool isFertilized;
    private GameObject currentPlant;

    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private PlantData plantInfo;

    public bool IsPlanted { get { return plant != null; } }

    private void Plant(PlantData data)
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

        currentPlant = Instantiate(plantData.stages[0], transform.position, Quaternion.identity, transform);

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

        Inventory.Instance.AddItem(new Item1(), 2);

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
        currentPlant = Instantiate(prefab, transform.position, Quaternion.identity, transform);

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            hasWater = true;
            Debug.Log("Regada");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Inventory.Instance.RemoveItem(new Item1()))
            {
                Plant(plantInfo);
                Debug.Log("Plantada");
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
         if(IsPlanted && plant.IsFullyGrown) 
            {
                Harvest();
                Debug.Log("Coshechada"); 
            }
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            Fertilize();
            Debug.Log("Fertilizada");
        }
    }
}
