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

        //[Serializable]
        //public struct Ingredient
        //{
        //    public string itemName;
        //    public int amount;
        //}

        //public Ingredient[] ingredients;

        public int cost;

        public string Id => throw new NotImplementedException();

        public Sprite uiSprite;

        public TowerType towerType;

        public enum TowerType : int
        {
            ATTACK,
            DEFENSE,
            UTILITY,
            COUNT
        }
    }
}