using UnityEngine;

public class PlantDatabase : MonoBehaviour
{
    private static PlantDatabase instance;
    public static PlantDatabase Instance {  get { return instance; } }

    [SerializeField] private PlantData[] plantDatas = new PlantData[0];
    public PlantData[] PlantDatas { get { return plantDatas; } }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(gameObject);
    }

    public PlantData GetPlantByName(string name)
    {
        foreach (var plantData in plantDatas)
            if(plantData.name == name) return plantData;

        return null;
    }
}
