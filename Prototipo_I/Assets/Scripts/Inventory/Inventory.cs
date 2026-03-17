using System;
using System.Collections.Generic;
using Saving;
using TMPro;
using TowerDefense;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils;
using static Inventory.Inventory;

namespace Inventory
{

    public class Inventory : Singleton<Inventory>, IAutoSaving<SeedCountWrapper>
    {
        #region IAutoSaving

        public float AutoSaveTime => 5.0f;

        public string File => "inventory.json";

        [SerializeField] private TextAsset defaultInventory;

        public SeedCountWrapper DefaultData
        {
            get
            {
                SeedCountWrapper data = JsonUtility.FromJson<SeedCountWrapper>(defaultInventory.text);
                return data;
            }
        }
        Action<float> ISaveable<SeedCountWrapper>.SaveEvent { get; set; }

        public SeedCountWrapper GetData() => new SeedCountWrapper(seedCount);
        public void SetData(SeedCountWrapper data) => seedCount = data.seedCount;

        public struct SeedCountWrapper
        {
            public int seedCount;

            public SeedCountWrapper(int seedCount)
            {
                this.seedCount = seedCount;
            }
        }

        #endregion

        [SerializeField] private int seedCount;

        [Header("Seed Counter UI")]
        [SerializeField] private TMP_Text seedCounterUI;

        private void Start()
        {
            InitSingleton();

            (this as IAutoSaving<SeedCountWrapper>).SetupAutoSave();
            (this as IAutoSaving<SeedCountWrapper>).Load();

            seedCounterUI.text = seedCount.ToString();
        }

        public int GetSeedCount() => seedCount;

        public void AddSeeds(int amount)
        {
            seedCount += amount;
            seedCounterUI.text = seedCount.ToString();
        }

        public bool RemoveSeeds(int amount) {
            if(seedCount < amount) return false;

            seedCount -= amount;
            seedCounterUI.text = seedCount.ToString();
            return true;
        }

        public bool HasSeeds(int amount) => seedCount >= amount;
    }
}