using System;
using UnityEngine;

namespace Farm
{
    [CreateAssetMenu(fileName = "PlantData", menuName = "Scriptable Objects/PlantData")]
    public class PlantData : ScriptableObject
    {
        public string plantName;
        public int timeToGrow;

        public GameObject[] stages;

        public GameObject plantWeapon;

        public int seedsPerRound;

    }
}