using System;
using UnityEngine;

namespace TowerDefense
{
    [CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]
    public class TowerData : ScriptableObject
    {
        public string Name;
        public float range = 15.0f;
        public GameObject[] stages;

        [Serializable]
        public struct Ingredient
        {
            public string itemName;
            public int amount;
        }

        public Ingredient[] ingredients;

        public Sprite uiSprite;

        public string Id => throw new NotImplementedException();
    }
}