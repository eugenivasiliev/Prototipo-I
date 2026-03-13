using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    [CreateAssetMenu(fileName = "TowerDB", menuName = "Scriptable Objects/Databases/Tower")]
    public class TowerDB : ScriptableObject
    {
        [SerializeField] private List<TowerData> towerDatas = new List<TowerData>();

        public List<TowerData> TowerDataList { get { return towerDatas; } }

        public Dictionary<string, TowerData> TowerDatas { get; private set; }

        public TowerData this[string s]
        {
            get => TowerDatas.GetValueOrDefault<string, TowerData>(s);
        }

        public void Init()
        {
            TowerDatas = new Dictionary<string, TowerData>();
            foreach (var towerData in towerDatas)
                TowerDatas.Add(towerData.Name, towerData);
        }
    }
}