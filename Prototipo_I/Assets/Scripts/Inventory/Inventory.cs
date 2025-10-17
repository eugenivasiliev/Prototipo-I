using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IAutoSaving<(Item, int)[]>
{
    private static Inventory instance;
    public static Inventory Instance { get { return instance; } }

    [SerializeField] private int inventorySpace;
    [SerializeField] private (Item, int)[] items = new (Item, int)[8];
    public (Item, int)[] Items { get => items; }

    #region IAutoSaving

    public float AutoSaveTime => 5.0f;

    public string File => "inventory.json";

    public (Item, int)[] DataToSave { get => items; set => items = value; }
    public UnityEvent<float> SaveEvent { get; set; }

    #endregion

    private void Start()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        (this as IAutoSaving<(Item, int)[]>).SetupAutoSave();

        Clear();

        AddItem(new Item1());
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
            if (items[i] != default && items[i].Item1.GetType() == item.GetType())
            {
                items[i].Item2++;
                GetImage(i).sprite = item.sprite;
                GetText(i).text = items[i].Item2.ToString();
                return true;
            }

        for (int i = 0; i < items.Length; i++)
            if (items[i] == default)
            {
                items[i] = (item, 1);
                GetImage(i).sprite = item.sprite;
                GetText(i).text = items[i].Item2.ToString();
                return true;
            }

        //Cannot add
        return false;
    }

    public bool AddItem(Item item, int amount)
    {
        for(int i = 0; i < amount; i++) if(!AddItem(item)) return false;
        return true;
    }

    public bool RemoveItem(Item item) {
        for (int i = 0; i < items.Length; i++)
            if (items[i] != default && items[i].Item1.GetType() == item.GetType())
            {
                items[i].Item2--;
                GetText(i).text = items[i].Item2.ToString();
                if (items[i].Item2 <= 0)
                {
                    items[i] = default;
                    GetImage(i).sprite = null;
                    GetText(i).text = "";
                }
                return true;
            }

        //Cannot remove
        return false;
    }

    public bool RemoveItem(Item item, int amount)
    {
        for (int i = 0; i < amount; i++) if (!RemoveItem(item)) return false;
        return true;
    }

    public void Clear() {
        for(int i = 0; i < items.Length; i++)
        {
            items[i] = default;
            GetImage(i).sprite = null;
            GetText(i).text = "";
        }
    }

    private Image GetImage(int i) => this.transform.GetChild(0).GetChild(i).GetComponent<Image>();
    private TMP_Text GetText(int i) => this.transform.GetChild(0).GetChild(i).GetComponentInChildren<TMP_Text>();
}
