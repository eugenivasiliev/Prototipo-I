using System;
using UnityEngine;

[Serializable]
public class Item
{
    public string Name; 
    public string Description;
    public Sprite sprite;

    public virtual void OnUse() {}
}
