using System;
using UnityEngine;

[Serializable]
public class Item1 : Item
{
    public override void OnUse(GameObject gameObject)
    {
        if(gameObject.TryGetComponent(out Plot plot))
        {
            plot.Plant(plot.plantInfo);
            Inventory.Instance.RemoveItem(this);
        }
    }
}
