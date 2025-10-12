using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour 
{
    private static Inventory instance;
    public static Inventory Instance { get { return instance; } }

    [SerializeField] private List<Item> items;

    public void AddItem(Item item) => items.Add(item);

    public void RemoveItem(Item item) => items.Remove(item);

    public void Clear() => items.Clear();

    public void Save()
    {
        FileManager.SaveFile(FileManager.InventoryFile, items);
    }

    public void Load()
    {
        FileManager.LoadFile(FileManager.InventoryFile, out items);
    }
}
