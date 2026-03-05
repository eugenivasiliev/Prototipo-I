using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantDB", menuName = "Scriptable Objects/Databases/Plant")]
public class PlantDB : ScriptableObject
{
    [SerializeField] private List<PlantData> plantDatas = new List<PlantData>();

    public Dictionary<string, PlantData> PlantDatas { get; private set; }

    public PlantData this[string s]
    {
        get => PlantDatas.GetValueOrDefault<string, PlantData>(s);
    } 

    public void Init()
    {
        PlantDatas = new Dictionary<string, PlantData>();
        foreach (var plantData in plantDatas)
            PlantDatas.Add(plantData.plantName, plantData);
    }
}