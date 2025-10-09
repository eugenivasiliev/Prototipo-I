using System.Timers;
using TMPro;
using UnityEngine;

public class Plot : MonoBehaviour
{
    Plant plant;
    PlantData plantData;

    private bool hasWater;
    private bool isFertilized;
    private float MultiplierSpeed;
    private GameObject currentPlant;

    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private PlantData plantInfo;

    public bool IsPlanted { get { return plant != null; } }

    private void Plant(PlantData data)
    {
        if (IsPlanted)
        {
            Debug.Log("Ya esta plantado");
            return;
        }

        plantData = data;
        plant = new Plant(data);

        currentPlant = Instantiate(plantData.stages[0], transform.position, Quaternion.identity, transform);

        plant.OnStageChanged += OnPlantStageChanged;

        hasWater = false;
        isFertilized = false;

        this.plant.Create();
        Debug.Log($"Planta {plant.Name} plantada!");
    }

    private void Harvest()
    {
        if (!IsPlanted || !plant.IsFullyGrown)
        {
            Debug.Log("Aun no esta lista");
            return;
        }
        Destroy(currentPlant);
        Debug.Log("Yw. Harvested");

        this.plant = null;
        currentPlant = null;
    }

    private void UpdateUI()
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

       if (currentPlant != null) { Destroy(currentPlant); }
        GameObject prefab = plantData.stages[currentStage];
        currentPlant = Instantiate(prefab, transform.position, Quaternion.identity, transform);

    }
    void Start()
    {
        //PlantManager.Instance.AssignPlot(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            hasWater = true;
            Debug.Log("Regada");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Plant(plantInfo);
            Debug.Log("Plantada");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
         if(IsPlanted && plant.IsFullyGrown) 
            {
                Harvest();
                Debug.Log("XD"); 
            }

        }
        UpdateUI();

    }

}
