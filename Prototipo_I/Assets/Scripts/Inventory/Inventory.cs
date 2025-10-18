using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public struct InventorySlot
{
    public Item item;
    public int amount;

    public InventorySlot(Item item, int amount) : this()
    {
        this.item = item;
        this.amount = amount;
    }

    public static InventorySlot Default = new InventorySlot(new Item(), -1);

    public static bool operator== (InventorySlot l, InventorySlot r)
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
        for(int i = 0; i < length; ++i)
        {
            slots[i] = InventorySlot.Default;
        }
    }
}

public class Inventory : MonoBehaviour, IAutoSaving<InventoryList>
{
    private static Inventory instance;
    public static Inventory Instance { get { return instance; } }

    [SerializeField] private int inventorySpace;
    public int InventorySpace { get { return inventorySpace; } }
    [SerializeField] private InventoryList items;

    #region IAutoSaving

    public float AutoSaveTime => 5.0f;

    public string File => "inventory.json";


    public UnityEvent<float> SaveEvent { get; set; }

    #endregion

    private Indicator indicator;

    private void Start()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        items = new InventoryList(inventorySpace);

        (this as IAutoSaving<InventoryList>).SetupAutoSave();
        (this as IAutoSaving<InventoryList>).Load();
        indicator = this.transform.GetChild(0).GetComponent<Indicator>();

    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < items.slots.Length; i++)
            if (items.slots[i] != InventorySlot.Default && items.slots[i].item.GetType() == item.GetType())
            {
                items.slots[i] = new InventorySlot(items.slots[i].item, items.slots[i].amount + 1);
                GetImage(i).sprite = item.sprite;
                GetText(i).text = items.slots[i].amount.ToString();
                return true;
            }

        for (int i = 0; i < items.slots.Length; i++)
            if (items.slots[i] == InventorySlot.Default)
            {
                items.slots[i] = new InventorySlot(item, 1);
                GetImage(i).sprite = item.sprite;
                GetText(i).text = items.slots[i].amount.ToString();
                return true;
            }

        //Cannot add
        return false;
    }

    public bool AddItem(Item item, int amount, out int amountDone)
    {
        for(amountDone = 1; amountDone <= amount; amountDone++) if(!AddItem(item)) return false;
        return true;
    }

    public bool RemoveItem(Item item) {
        for (int i = 0; i < items.slots.Length; i++)
            if (items.slots[i] != null && items.slots[i].item.GetType() == item.GetType())
            {
                items.slots[i] = new InventorySlot(items.slots[i].item, items.slots[i].amount - 1);
                GetText(i).text = items.slots[i].amount.ToString();
                if (items.slots[i].amount <= 0)
                {
                    items.slots[i] = InventorySlot.Default;
                    GetImage(i).sprite = null;
                    GetText(i).text = "";
                }
                return true;
            }

        //Cannot remove
        return false;
    }

    public bool RemoveItem(Item item, int amount, out int amountDone)
    {
        for (amountDone = 1; amountDone <= amount; amountDone++) if (!RemoveItem(item)) return false;
        return true;
    }

    public void Clear() {
        for(int i = 0; i < items.slots.Length; i++)
        {
            items.slots[i] = InventorySlot.Default;
            GetImage(i).sprite = null;
            GetText(i).text = "";
        }
    }

    public void UseCurrentItem(GameObject gameObject)
    {
        items.slots[indicator.CurrentIndex].item?.OnUse(gameObject);
    }

    public Item GetCurrentItem()
    {
        return items.slots[indicator.CurrentIndex].item ?? default;
    }

    public Vector2 GetItemUIPosition(int i) => this.transform.GetChild(1).GetChild(i).GetComponent<RectTransform>().position;
    private Image GetImage(int i) => this.transform.GetChild(1).GetChild(i).GetComponent<Image>();
    private TMP_Text GetText(int i) => this.transform.GetChild(1).GetChild(i).GetComponentInChildren<TMP_Text>();

    public InventoryList GetData()
    {
        Debug.Log(items.slots.Length);
        return items;
    }

    public void SetData(InventoryList data)
    {
        items = data;
    }
}
