using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerDB", menuName = "Scriptable Objects/Databases/Tower")]
public class TowerDB : ScriptableObject
{
    [SerializeField] private List<TowerData> towerDatas = new List<TowerData>();

    public List<TowerData> TowerDataList { get { return towerDatas; } }

    public Dictionary<TowerData.TowerType, List<TowerData>> filteredDatas { get; private set; }

    public Dictionary<string, TowerData> TowerDatas { get; private set; }

    public TowerData this[string s]
    {
        get => TowerDatas.GetValueOrDefault<string, TowerData>(s);
    }

    public void Init()
    {
        TowerDatas = new Dictionary<string, TowerData>();
        filteredDatas = new Dictionary<TowerData.TowerType, List<TowerData>>();

        for(int i = 0; i < (int)TowerData.TowerType.COUNT; i++)
            filteredDatas.Add((TowerData.TowerType)i, new List<TowerData>());

        foreach (var towerData in towerDatas)
        {
            TowerDatas.Add(towerData.Name, towerData);
            filteredDatas[towerData.towerType].Add(towerData);
        }
    }
}