using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HybridationManager", menuName = "Scriptable Objects/HybridationManager")]
public class HybridationManager : ScriptableObject
{
    [Serializable]
    public struct HybridTuple
    {
        [SerializeField] public string Name;
        [SerializeReference] public PlantData parent1; 
        [SerializeReference] public PlantData parent2;
        [SerializeReference] public PlantData child;

        public readonly bool Equals((PlantData, PlantData) pair)
        {
            return (pair.Item1 == parent1 && pair.Item2 == parent2) || (pair.Item1 == parent2 && pair.Item2 == parent1);
        }
    }

    public List<HybridTuple> hybridList = new List<HybridTuple>();

    public bool TryFindHybrid((PlantData, PlantData) pair, out PlantData plantData)
    {
        plantData = null;
        foreach (var hybrid in hybridList)
            if (hybrid.Equals(pair))
            {
                plantData = hybrid.child;
                return true;
            }
        return false;
    }
}
