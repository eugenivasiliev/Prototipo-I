using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using Utils;

namespace Enemies
{
    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private string deathSound;
        [SerializeField] private AnimationClip deathAnim;
        private float deathAnimTime;

        [Serializable]
        public struct DropRateObject
        {
            public GameObject gameObject;
            public float rate;

            public DropRateObject(GameObject gameObject, float rate)
            {
                this.gameObject = gameObject;
                this.rate = rate;
            }
        }

        [Header("Loot")]
        [SerializeField] protected List<DropRateObject> droppableLoot;
        [SerializeField] protected int minItemsDropped;
        [SerializeField] protected int maxItemsDropped;
        [SerializeField, Range(0, 5)] protected float dropRadius;
        [SerializeField, Range(0, 5)] protected float dropHeight = 2;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            deathAnimTime = deathAnim.length;
        }

        // Update is called once per frame
        void Update()
        {
            AudioManager.Instance.PlaySFXEvent(deathSound);

            deathAnimTime -= Time.deltaTime;

            if (deathAnimTime > 0) return;

            DropLoot();
            Destroy(gameObject);
        }

        protected void DropLoot()
        {
            int itemsDropped = UnityEngine.Random.Range(minItemsDropped, maxItemsDropped + 1);
            for (int i = 0; i < itemsDropped; ++i)
                DropLootItem();
        }

        protected void DropLootItem()
        {
            Vector2 dropSpot = dropRadius * UnityEngine.Random.insideUnitCircle;
            float lootDropped = UnityEngine.Random.value;
            foreach (DropRateObject drop in droppableLoot)
                if (drop.rate > lootDropped)
                {
                    GameObject loot = Instantiate(drop.gameObject, this.transform.position, Quaternion.identity);
                    TweenMovement lootMovement = loot.GetComponent<TweenMovement>();
                    return;
                }
        }
    }
}