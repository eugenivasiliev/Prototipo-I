using System;
using UnityEngine;

[Serializable]
public class Item
{
    public string Name; 
    public string Description;

    public virtual void OnUse() {}
}
