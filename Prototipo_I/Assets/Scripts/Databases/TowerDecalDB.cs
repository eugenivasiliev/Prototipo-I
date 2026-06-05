using System.Collections.Generic;
using TowerDefense;
using UnityEngine;

namespace TowerDefense
{
    [CreateAssetMenu(fileName = "TowerDecalDB", menuName = "Scriptable Objects/Databases/TowerDecalDB")]
    public class TowerDecalDB : ScriptableObject
    {
        [SerializeField] private List<GameObject> towerDecals = new List<GameObject>();

        public List<GameObject> TowerDecalList { get { return towerDecals; } }

        public Dictionary<string, GameObject> TowerDecals { get; private set; }

        public GameObject this[string s]
        {
            get => TowerDecals[s];
        }

        public void Init()
        {
            TowerDecals = new Dictionary<string, GameObject>();

            foreach (var towerDecal in towerDecals)
            {
                TowerDecals.Add(towerDecal.name, towerDecal);
                
            }
        }
    }
}