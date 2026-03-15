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
    [Serializable]
    public struct InventorySlot
    {
        [SerializeReference] public Item item;
        public int amount;

        public InventorySlot(Item item, int amount) : this()
        {
            this.item = item;
            this.amount = amount;
        }

        public static InventorySlot Default = new InventorySlot(new Item(), -1);

        public static bool operator ==(InventorySlot l, InventorySlot r)
        {
            return l.item.GetType() == r.item.GetType() && l.amount == r.amount;
        }

        public static bool operator !=(InventorySlot l, InventorySlot r)
        {
            return !(l == r);
        }

        public override bool Equals(object obj)
        {
            return obj is InventorySlot slot &&
                   EqualityComparer<System.Type>.Default.Equals(item.GetType(), slot.item.GetType()) &&
                   amount == slot.amount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(item, amount);
        }
    }

    [Serializable]
    public struct InventoryList
    {
        public InventorySlot[] slots;

        public InventoryList(int length)
        {
            slots = new InventorySlot[length];
            for (int i = 0; i < length; ++i)
            {
                slots[i] = InventorySlot.Default;
            }
        }
    }

    public class Inventory : Singleton<Inventory>, IAutoSaving<SeedCountWrapper>
    {

        [SerializeField] private int inventorySpace;
        public int InventorySpace { get { return inventorySpace; } }
        [SerializeField] private InventoryList items;

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

        public UnityEvent<float> SaveEvent { get; set; }

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

        private Indicator indicator;

        [SerializeField] private int seedCount;

        [Header("UI Sprites")]
        [SerializeField] private UISpritesDB uiSpritesDB;
        [SerializeField] private Sprite defaultItemSprite;

        [Header("Seed Counter UI")]
        [SerializeField] private TMP_Text seedCounterUI;

        private void Start()
        {
            InitSingleton();
            uiSpritesDB.Init();

            items = new InventoryList(inventorySpace);

            (this as IAutoSaving<SeedCountWrapper>).SetupAutoSave();
            (this as IAutoSaving<SeedCountWrapper>).Load();

            seedCounterUI.text = seedCount.ToString();

            //for (int i = 0; i < inventorySpace; ++i)
            //{
            //    if (items.slots[i] == InventorySlot.Default) DefaultItem(i);
            //    else RenderItem(i);
            //}

            //indicator = this.transform.GetChild(0).GetComponent<Indicator>();
            //indicator.Initialize(this.transform.GetComponentInChildren<GridLayoutGroup>().cellSize);

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

        public int GetItemCount(Item item)
        {
            for (int i = 0; i < items.slots.Length; i++)
                if (items.slots[i].item.GetType() == item.GetType())
                {
                    return items.slots[i].amount;
                }
            return 0;
        }

        public int GetItemCount(string itemName)
        {
            for (int i = 0; i < items.slots.Length; i++)
                if (items.slots[i].item.Id == itemName)
                {
                    return items.slots[i].amount;
                }
            return 0;
        }

        public bool AddItem(Item item)
        {
            for (int i = 0; i < items.slots.Length; i++)
                if (items.slots[i] != InventorySlot.Default && items.slots[i].item.GetType() == item.GetType())
                {
                    items.slots[i].amount += 1;
                    RenderItem(i);
                    return true;
                }

            for (int i = 0; i < items.slots.Length; i++)
                if (items.slots[i] == InventorySlot.Default)
                {
                    items.slots[i] = new InventorySlot(item, 1);
                    RenderItem(i);
                    return true;
                }

            //Cannot add
            return false;
        }

        public bool AddItem(Item item, int amount, out int amountDone)
        {
            for (amountDone = 1; amountDone <= amount; amountDone++) if (!AddItem(item)) return false;
            return true;
        }

        public bool RemoveItem(Item item)
        {
            for (int i = 0; i < items.slots.Length; i++)
                if (items.slots[i] != null && items.slots[i].item.GetType() == item.GetType())
                {
                    items.slots[i] = new InventorySlot(items.slots[i].item, items.slots[i].amount - 1);
                    if (items.slots[i].amount <= 0) DefaultItem(i);
                    else RenderItem(i);
                    return true;
                }

            //Cannot remove
            return false;
        }

        public bool RemoveItem(string itemName)
        {
            for (int i = 0; i < items.slots.Length; i++)
                if (items.slots[i].item.Id == itemName)
                {
                    return RemoveItem(items.slots[i].item);
                }

            //Cannot remove
            return false;
        }

        public bool RemoveItem(Item item, int amount, out int amountDone)
        {
            for (amountDone = 1; amountDone <= amount; amountDone++) if (!RemoveItem(item)) return false;
            return true;
        }

        public bool RemoveItem(string itemName, int amount, out int amountDone)
        {
            for (amountDone = 1; amountDone <= amount; amountDone++) if (!RemoveItem(itemName)) return false;
            return true;
        }

        public void Clear()
        {
            for (int i = 0; i < items.slots.Length; i++)
            {
                items.slots[i] = InventorySlot.Default;
                GetImage(i).sprite = null;
                GetText(i).text = "";
            }
        }

        private void RenderItem(int index)
        {
            GetText(index).text = items.slots[index].amount.ToString();
            GetImage(index).sprite = uiSpritesDB[items.slots[index].item.Name];
        }

        private void DefaultItem(int index)
        {
            items.slots[index] = InventorySlot.Default;
            GetImage(index).sprite = defaultItemSprite;
            GetText(index).text = "";
        }

        public Item GetCurrentItem() => (indicator.CurrentIndex == -1) ? null : items.slots[indicator.CurrentIndex].item;
        public Vector2 GetItemUIPosition(int i) => this.transform.GetChild(1).GetChild(i).GetComponent<RectTransform>().position;
        private Image GetImage(int i) => this.transform.GetChild(1).GetChild(i).GetComponent<Image>();
        private TMP_Text GetText(int i) => this.transform.GetChild(1).GetChild(i).GetComponentInChildren<TMP_Text>();

        //public bool HasIngredients(TowerData.Ingredient[] ingredients)
        //{
        //    foreach (TowerData.Ingredient ingredient in ingredients)
        //    {
        //        if (GetItemCount(ingredient.itemName) < ingredient.amount)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
    }
}