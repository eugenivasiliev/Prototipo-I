using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour 
{
    private static Inventory instance;
    public static Inventory Instance { get { return instance; } }

    [SerializeField] public int inventorySpace { get => 8; }
    [SerializeField] private (Item, int)[] items = new (Item, int)[8];

    private Indicator indicator;

    private void Start()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        Clear();

        AddItem(new Item1());

        indicator = this.transform.GetChild(0).GetComponent<Indicator>();
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

    public bool AddItem(Item item, int amount, out int amountDone)
    {
        for(amountDone = 1; amountDone <= amount; amountDone++) if(!AddItem(item)) return false;
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

    public bool RemoveItem(Item item, int amount, out int amountDone)
    {
        for (amountDone = 1; amountDone <= amount; amountDone++) if (!RemoveItem(item)) return false;
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

    public void UseCurrentItem(GameObject gameObject)
    {
        items[indicator.CurrentIndex].Item1?.OnUse(gameObject);
    }

    public Item GetCurrentItem()
    {
        return items[indicator.CurrentIndex].Item1 ?? default;
    }

    public Vector2 GetItemUIPosition(int i) => this.transform.GetChild(1).GetChild(i).GetComponent<RectTransform>().position;
    private Image GetImage(int i) => this.transform.GetChild(1).GetChild(i).GetComponent<Image>();
    private TMP_Text GetText(int i) => this.transform.GetChild(1).GetChild(i).GetComponentInChildren<TMP_Text>();

    public void Save()
    {
        FileManager.SaveFile(FileManager.InventoryFile, items);
    }

    public void Load()
    {
        FileManager.LoadFile(FileManager.InventoryFile, out items);
    }
}
