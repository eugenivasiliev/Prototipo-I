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
        [SerializeField] private Color defaultColour;
        [SerializeField] private Color emptyColour;

        [Header("Cheats")]
        [SerializeField] private int cheatAddedSeeds = 10;

        private void Start()
        {
            InitSingleton();

            //Autosave disabled because of scope cuts
            //Will be implemented in a future sprint 
            (this as IAutoSaving<SeedCountWrapper>).LoadDefault();

            seedCounterUI.text = seedCount.ToString();

            InputSystem.actions.FindAction("debug").performed += ctx => { this.AddSeeds(cheatAddedSeeds); };
        }

        public int GetSeedCount() => seedCount;

        public void AddSeeds(int amount)
        {
            seedCount += amount;
            seedCounterUI.text = seedCount.ToString();
            seedCounterUI.color = (seedCount == 0) ? emptyColour : defaultColour;
        }

        public bool RemoveSeeds(int amount) {
            if(seedCount < amount) return false;

            seedCount -= amount;
            seedCounterUI.text = seedCount.ToString();
            seedCounterUI.color = (seedCount == 0) ? emptyColour : defaultColour;
            return true;
        }

        public bool HasSeeds(int amount) => seedCount >= amount;

        public bool HasSeeds() => seedCount > 0;
    }
}