using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] protected GameObject projectile;
        protected bool attacking = false;
        protected List<GameObject> closeEnemies = new List<GameObject>();
        protected GameObject targetedEnemy;

        protected bool tracking = true;
        protected float speed = 4.5f;
        [SerializeField] protected float waitTime = 0.6f;
        [SerializeField, Range(0, 50)] protected float maxRange = 15;
        [SerializeField, Range(0, 50)] protected float minRange = 3;

        [Header("Animation")]
        [SerializeField] protected Animator animator;

        public float GetRange()
        {
            return maxRange;
        }
    }
}