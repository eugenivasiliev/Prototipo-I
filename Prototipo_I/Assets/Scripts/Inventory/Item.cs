using System;
using UnityEngine;

[Serializable]
public class Item
{
    public string Name; 
    public string Description;
    public virtual string spriteId { get => ""; }

    public virtual void OnUse(GameObject gameObject) {}
}
