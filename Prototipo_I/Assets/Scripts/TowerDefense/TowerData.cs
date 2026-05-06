using System;
using UnityEngine;

namespace TowerDefense
{
    [CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]
    public class TowerData : ScriptableObject
    {
        public string Name;
        public string Description;
        public float range = 0.0f;
        public GameObject[] stages;

        public int cost;
        public int damage = 0;
        public bool hasAOE = false;

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