using System;
using System.Collections.Generic;
using UnityEngine;

namespace Farm
{
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

            public readonly bool Equals(PlantData p1, PlantData p2)
            {
                return (p1 == parent1 && p2 == parent2) || (p1 == parent2 && p2 == parent1);
            }
        }

        public List<HybridTuple> hybridList = new List<HybridTuple>();

        public bool TryFindHybrid(PlantData p1, PlantData p2, out Farm.PlantData plantData)
        {
            plantData = null;
            foreach (var hybrid in hybridList)
                if (hybrid.Equals(p1, p2))
                {
                    plantData = hybrid.child;
                    return true;
                }
            return false;
        }
    }
}