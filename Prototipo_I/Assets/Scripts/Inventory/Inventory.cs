using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour 
{
    private static Inventory instance;
    public static Inventory Instance { get { return instance; } }

    [SerializeField] private int inventorySpace;
    [SerializeField] private (Item, int)[] items = new (Item, int)[8];

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
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
            if (items[i].Item1 == item)
            {
                items[i].Item2++;
                this.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = item.sprite;
                this.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = items[i].Item2.ToString();
                return true;
            }

        for (int i = 0; i < items.Length; i++)
            if (items[i] == default)
            {
                items[i] = (item, 1);
                this.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = item.sprite;
                this.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = items[i].Item2.ToString();
                return true;
            }

        //Cannot add
        return false;
    }

    public bool RemoveItem(Item item) {
        for (int i = 0; i < items.Length; i++)
            if (items[i].Item1 == item)
            {
                items[i].Item2--;
                this.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = items[i].Item2.ToString();
                if (items[i].Item2 <= 0)
                {
                    items[i] = default;
                    this.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = null;
                    this.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = "";
                }
                return true;
            }

        //Cannot remove
        return false;
    } 

    public void Clear() {
        for(int i = 0; i < items.Length; i++)
        {
            items[i] = default;
            this.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = null;
            this.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = "";
        }
    }

    public void Save()
    {
        FileManager.SaveFile(FileManager.InventoryFile, items);
    }

    public void Load()
    {
        FileManager.LoadFile(FileManager.InventoryFile, out items);
    }
}
